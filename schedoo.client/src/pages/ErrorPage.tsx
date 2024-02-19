import { useLocation } from "react-router-dom";

export function ErrorPage() {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const errorMessage = queryParams.get("message") || "An error occurred.";

    return (
        <div>
            <h1>Error</h1>
            <p>{errorMessage}</p>
        </div>
    );
}