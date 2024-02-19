import { useLocation, useNavigate } from "react-router-dom";

export function ResponseStatus401() {
    const message = "You are not authorized";
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";
    const navigate = useNavigate();

    function toLogin() {
        navigate(`/login`);
    }

    function back() {
        navigate(from, { replace: true });
    }
    
    return (
        <section>
            <h4>{message}</h4>
            <a href="#" onClick={toLogin}>Log in</a>
            <a href="#" onClick={back}>Back</a>
        </section>
    )
}