import { useAuth } from "../../hooks/useAuth"

export function Profile() {
    const { auth } = useAuth();

    return (
        <>
            <p>{auth?.username}</p>
            <p>{auth?.name}</p>
            <p>{auth?.surname}</p>
            <p>{auth?.patronymic}</p>
            <p>{auth?.phoneNumber}</p>
        </>
    )
}