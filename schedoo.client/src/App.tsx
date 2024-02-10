import { Route, Routes } from 'react-router-dom';
import { Schedule } from "./pages/Schedule.tsx";
import { AttendancePage } from './pages/AttendancePage.tsx';
import Register from './pages/auth/Register.tsx';
import { Login } from './pages/auth/Login.tsx';
import './App.css';
import RequireAuth from './pages/auth/RequireAuth.tsx';

function App() {
    return (
        <Routes>
            <Route path="/" element={<Schedule />} />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />

            {/* <Route element={<RequireAuth/>}> */}
                <Route path="/attendance/:scheduleDateId" element={<AttendancePage />} />
            {/* </Route> */}
        </Routes>
    );
}

export default App;