import { useContext, useEffect } from "react";
import AuthContext from "../context/AuthProvider.tsx";

export function useAuth() {
    const { auth, setAuth } = useContext(AuthContext);

    // Function to update authentication state and store it in localStorage
    const updateAuth = (newAuth) => {
        setAuth(newAuth);
        localStorage.setItem('auth', JSON.stringify(newAuth));
    };

    // Initialize authentication state from localStorage on component mount
    useEffect(() => {
        const storedAuth = localStorage.getItem('auth');
        if (storedAuth) {
            setAuth(JSON.parse(storedAuth));
        }
    }, []);

    return { auth, setAuth: updateAuth };
}