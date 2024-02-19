import { Route, Routes, useLocation, useNavigate } from 'react-router-dom';
import { Schedule } from "./pages/Schedule.tsx";
import { AttendancePage } from './pages/AttendancePage.tsx';
import Register from './pages/auth/Register.tsx';
import { Login } from './pages/auth/Login.tsx';
import './App.css';
import RequireAuth from './pages/auth/RequireAuth.tsx';
// import RequireAuth from './pages/auth/RequireAuth.tsx';

function App() {
    const location = useLocation();
    const navigate = useNavigate();

    const queryParams = new URLSearchParams(location.search);
    const prevSemesterId = parseInt(queryParams.get("semesterId") || "", 10);

    return (
        <Routes>
            <Route
                path="/schedule"
                element={<Schedule prevSemesterId={prevSemesterId} />}
            />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />

            <Route element={<RequireAuth/>}>
                <Route path="/attendance/:scheduleDateId" element={<AttendancePage />} />
            </Route>
        </Routes>
    );
}

export default App;