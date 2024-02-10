import { useContext } from "react";
import AuthContext from "../context/AuthProvider.tsx";

export function useAuth() {
    return useContext(AuthContext);
}