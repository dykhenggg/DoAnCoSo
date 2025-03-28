function SigninPage() {
    return (
        <div class="body-signin">
            <div class="wrapper">
                <form action="">
                    <h1> Staff Login</h1>
                    <div class="input-box">
                        <input type="text" placeholder="Username" required />
                        <i class='bx bxs-user'></i>
                    </div>
                    <div class="input-box">
                        <input type="password" placeholder="Password" required />
                        <i class='bx bxs-lock-alt' ></i>
                    </div>
                    <div class="remember-forgot">
                        <label><input type="checkbox" />Remember Me</label>
                        <a href="#">Forgot Password</a>
                    </div>
                    <button type="submit" class="btn">Login</button>
                    <div class="register-link">
                        <p>Dont have an account? <a href="#">Register</a></p>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default SigninPage