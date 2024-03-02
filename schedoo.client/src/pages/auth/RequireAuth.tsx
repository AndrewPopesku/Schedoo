import { useLocation, Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { useEffect, useState } from "react";
import { CircularProgress } from "@mui/material";

const RequireAuth = ({allowedRoles}: {allowedRoles?: string[]}) => {
    const { auth } = useAuth();
    const location = useLocation();
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        setLoading(false);
    }, [auth])

    const content = (
        auth && (!allowedRoles || auth.roles?.find(role => allowedRoles.includes(role))) ?
            <Outlet />
        : auth?.email ? 
            <Navigate to="/unauthorized" state={{ from: location }} replace />
            :
            <Navigate to="/login" state={{ from: location }} replace />
    );

    return loading ? <CircularProgress/> : content;
}

export default RequireAuth;