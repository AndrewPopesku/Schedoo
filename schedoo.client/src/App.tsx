import { Route, Routes, useLocation, useNavigate } from 'react-router-dom';
import { AttendancePage } from './pages/AttendancePage.tsx';
import Register from './pages/auth/Register.tsx';
import { Login } from './pages/auth/Login.tsx';
import './App.css';
import RequireAuth from './pages/auth/RequireAuth.tsx';
import { Profile } from './pages/auth/Profile.tsx';
import { SchedulePage } from './pages/SchedulePage.tsx';

function App() {
    const location = useLocation();
    const navigate = useNavigate();

    const queryParams = new URLSearchParams(location.search);
    const prevSemesterId = parseInt(queryParams.get("semesterId") || "", 10);

    return (
        <Routes>
            <Route
                path="/schedule"
                element={<SchedulePage prevSemesterId={prevSemesterId} />}
            />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />
            <Route path="profile" element={<Profile/>}/>

            <Route element={<RequireAuth allowedRoles={["Administrator", "Group Leader"]}/>}>
                <Route path="/attendance/:scheduleDateId" element={<AttendancePage />} />
            </Route>
        </Routes>
    );
}

export default App;