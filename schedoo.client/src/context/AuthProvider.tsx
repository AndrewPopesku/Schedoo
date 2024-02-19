import { createContext, useState } from "react";
import { User } from "../types/interfaces";

const AuthContext = createContext();

export function AuthProvider({ children }) {
    const [auth, setAuth] = useState<User>({});

    return (
        <AuthContext.Provider value={{ auth, setAuth }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;