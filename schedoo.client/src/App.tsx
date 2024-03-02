import { Route, Routes } from 'react-router-dom';
import { AttendancePage } from './pages/AttendancePage.tsx';
import Register from './pages/auth/Register.tsx';
import { Login } from './pages/auth/Login.tsx';
import './App.css';
import RequireAuth from './pages/auth/RequireAuth.tsx';
import { Profile } from './pages/auth/Profile.tsx';
import { SchedulePage } from './pages/SchedulePage.tsx';
import { Unauthorized } from './pages/auth/Unauthorized.tsx';
import { AuthProvider } from './context/AuthProvider.tsx';
import { useAuth } from './hooks/useAuth.tsx';

function App() {
    const { auth } = useAuth(null);

    return (
        <AuthProvider initialAuth={auth}>
            <Routes>
                <Route path="/register" element={<Register />} />
                <Route path="/login" element={<Login />} />
                <Route path="/unauthorized" element={<Unauthorized/>}/>

                <Route element={<RequireAuth allowedRoles={["Administrator", "Group Leader", "Student"]}/>}>
                    <Route path="/profile" element={<Profile/>}/>
                </Route>

                <Route path="/schedule" element={<SchedulePage />}/>

                <Route element={<RequireAuth allowedRoles={["Administrator", "Group Leader"]}/>}>
                    <Route path="/attendance/:scheduleDateId" element={<AttendancePage />} />
                </Route>
            </Routes>
        </AuthProvider>
    );
}

export default App;