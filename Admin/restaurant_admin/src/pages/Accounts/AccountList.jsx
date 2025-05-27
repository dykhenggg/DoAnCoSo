import React, { useState, useEffect } from "react";
import { Table, Button, Input, Select, Modal, Form, message } from "antd";
import { EditOutlined, LockOutlined, UnlockOutlined } from "@ant-design/icons";
import "./Accounts.css";

const { Search } = Input;
const { Option } = Select;

const AccountList = () => {
  const [accounts, setAccounts] = useState([]);
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingAccount, setEditingAccount] = useState(null);
  const [form] = Form.useForm();

  // Lấy danh sách tài khoản và vai trò khi component được mount
  useEffect(() => {
    fetchAccounts();
    fetchRoles();
  }, []);

  // Lấy danh sách tài khoản từ API
  const fetchAccounts = async () => {
    setLoading(true);
    try {
      const response = await fetch("http://localhost:5078/api/NhanVien");
      const data = await response.json();
      setAccounts(data);
    } catch (error) {
      message.error("Không thể tải danh sách tài khoản");
    }
    setLoading(false);
  };

  // Lấy danh sách vai trò từ API
  const fetchRoles = async () => {
    try {
      const response = await fetch("http://localhost:5078/api/Role");
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      if (!Array.isArray(data) || data.length === 0) {
        message.warning("Không có vai trò nào trong hệ thống");
        return;
      }
      setRoles(data);
    } catch (error) {
      console.error("Error fetching roles:", error);
      message.error("Không thể tải danh sách vai trò");
    }
  };

  // Xử lý tìm kiếm
  const handleSearch = (value) => {
    // Thực hiện tìm kiếm theo tên hoặc email
  };

  // Xử lý lọc theo vai trò
  const handleRoleFilter = (value) => {
    // Thực hiện lọc theo vai trò
  };

  // Xử lý lọc theo trạng thái
  const handleStatusFilter = (value) => {
    // Thực hiện lọc theo trạng thái
  };

  // Mở modal thêm/sửa tài khoản
  const showModal = (account = null) => {
    setEditingAccount(account);
    if (account) {
      form.setFieldsValue(account);
    } else {
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  // Xử lý lưu tài khoản
  const handleSave = async (values) => {
    try {
      const url = editingAccount
        ? `http://localhost:5078/api/NhanVien/${editingAccount.maNV}`
        : "http://localhost:5078/api/NhanVien";
      const method = editingAccount ? "PUT" : "POST";

      const response = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(values),
      });

      if (response.ok) {
        message.success(
          `${editingAccount ? "Cập nhật" : "Thêm"} tài khoản thành công`
        );
        setIsModalVisible(false);
        fetchAccounts();
      }
    } catch (error) {
      message.error("Có lỗi xảy ra");
    }
  };

  // Xử lý khóa/mở khóa tài khoản
  const handleToggleStatus = async (account) => {
    try {
      const newStatus = account.trangThai === "Active" ? "Inactive" : "Active";
      const response = await fetch(`/api/users/${account.userId}/status`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ trangThai: newStatus }),
      });

      if (response.ok) {
        message.success("Cập nhật trạng thái thành công");
        fetchAccounts();
      }
    } catch (error) {
      message.error("Có lỗi xảy ra");
    }
  };

  // Cấu hình các cột cho bảng
  const columns = [
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
    },
    {
      title: "Họ tên",
      dataIndex: "hoTen",
      key: "hoTen",
    },
    {
      title: "Vai trò",
      dataIndex: ["vaiTro", "tenVaiTro"],
      key: "vaiTro",
    },
    {
      title: "Trạng thái",
      dataIndex: "trangThai",
      key: "trangThai",
      render: (text) => (
        <span className={`status-tag ${text.toLowerCase()}`}>
          {text === "Active" ? "Hoạt động" : "Đã khóa"}
        </span>
      ),
    },
    {
      title: "Thao tác",
      key: "action",
      render: (_, record) => (
        <div className="action-buttons">
          <Button icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Button
            icon={
              record.trangThai === "Active" ? (
                <LockOutlined />
              ) : (
                <UnlockOutlined />
              )
            }
            onClick={() => handleToggleStatus(record)}
            danger={record.trangThai === "Active"}
          />
        </div>
      ),
    },
  ];

  return (
    <div className="accounts-container">
      <div className="accounts-header">
        <div className="header-left">
          <h2>Quản lý tài khoản</h2>
          <span className="total-count">{accounts.length} tài khoản</span>
        </div>
        <div className="header-right">
          <Search
            placeholder="Tìm kiếm theo tên hoặc email"
            onSearch={handleSearch}
            className="search-input"
          />
          <Select
            placeholder="Lọc theo vai trò"
            onChange={handleRoleFilter}
            className="filter-select"
          >
            {roles.map((role) => (
              <Option key={role.maVaiTro} value={role.maVaiTro}>
                {role.tenVaiTro}
              </Option>
            ))}
          </Select>
          <Select
            placeholder="Lọc theo trạng thái"
            onChange={handleStatusFilter}
            className="filter-select"
          >
            <Option value="Active">Hoạt động</Option>
            <Option value="Inactive">Đã khóa</Option>
          </Select>
          <Button type="primary" onClick={() => showModal()}>
            Thêm tài khoản
          </Button>
        </div>
      </div>

      <Table
        columns={columns}
        dataSource={accounts}
        loading={loading}
        rowKey="userId"
        className="accounts-table"
      />

      <Modal
        title={editingAccount ? "Chỉnh sửa tài khoản" : "Thêm tài khoản mới"}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        footer={null}
      >
        <Form form={form} layout="vertical" onFinish={handleSave}>
          <Form.Item
            name="hoTen"
            label="Họ tên"
            rules={[{ required: true, message: "Vui lòng nhập họ tên" }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="email"
            label="Email"
            rules={[
              { required: true, message: "Vui lòng nhập email" },
              { type: "email", message: "Email không hợp lệ" },
            ]}
          >
            <Input />
          </Form.Item>

          {!editingAccount && (
            <Form.Item
              name="matKhau"
              label="Mật khẩu"
              rules={[{ required: true, message: "Vui lòng nhập mật khẩu" }]}
            >
              <Input.Password />
            </Form.Item>
          )}

          <Form.Item
            name="maVaiTro"
            label="Vai trò"
            rules={[{ required: true, message: "Vui lòng chọn vai trò" }]}
          >
            <Select>
              {roles.map((role) => (
                <Option key={role.maVaiTro} value={role.maVaiTro}>
                  {role.tenVaiTro}
                </Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item className="form-actions">
            <Button onClick={() => setIsModalVisible(false)}>Hủy</Button>
            <Button type="primary" htmlType="submit">
              {editingAccount ? "Cập nhật" : "Thêm mới"}
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default AccountList;
