import { createContext, useState } from "react";
import { Auth } from "../types/interfaces";

const AuthContext = createContext<{ auth: Auth | undefined; setAuth: (newAuth: Auth) => void } | null>(null);

export function AuthProvider({ children }) {
    const [auth, setAuth] = useState<Auth>();

    return (
        <AuthContext.Provider value={{ auth, setAuth }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;