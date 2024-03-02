import { useContext, useEffect } from "react";
import AuthContext from "../context/AuthProvider.tsx";
import { Auth } from "../types/interfaces.ts";

export function useAuth() {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }

    const { auth, setAuth } = context;

    // Function to update authentication state and store it in localStorage
    const updateAuth = (newAuth: Auth) => {
        setAuth(newAuth);
        localStorage.setItem('auth', JSON.stringify(newAuth));
    };

    // Initialize authentication state from localStorage on component mount
    useEffect(() => {
        const storedAuth = localStorage.getItem('auth');
        if (storedAuth) {
            setAuth(JSON.parse(storedAuth));
        } else {
            setAuth({});
        }
    }, []);

    return { auth, setAuth: updateAuth };
}