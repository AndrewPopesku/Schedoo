import { useEffect, useState } from "react";
import { WeekType } from "../types/enums.ts";
import { Group, Semester } from "../types/interfaces.ts";
import { Autocomplete, CircularProgress, Container, TextField, ToggleButton, ToggleButtonGroup } from "@mui/material";
import { ScheduleTable } from "../containers/ScheduleTable.tsx";
import '../styles/Schedule.css';
import { useQueryState } from "../hooks/useQueryState.tsx";
import axios from "../api/axios.ts";
import { AxiosResponse, isAxiosError } from "axios";
import { getGroupsBySemesterIdReq, getSemestersReq } from "../api/requests.ts";
import { useAuth } from "../hooks/useAuth.tsx";
import { useLocation, useNavigate } from "react-router-dom";

export function SchedulePage() {
    const { auth, setAuth } = useAuth();
    const location = useLocation();
    const navigate = useNavigate();

    const [weekType, setWeekType] = useState<WeekType>();
    const [currentWeekType, setCurrentWeekType] = useState<WeekType>();

    const [groups, setGroups] = useState<Group[]>([]);
    const [selectedGroupName, setSelectedGroupName] = useQueryState("groupName");
    const [openG, setOpenG] = useState<boolean>(false);

    const [semesters, setSemesters] = useState<Semester[]>([]);
    const [selectedSemesterId, setSelectedSemesterId] = useQueryState("semesterId");
    const [openS, setOpenS] = useState<boolean>(false);

    useEffect(() => {
        populateSemesterData();
    }, []);

    useEffect(() => {
        setGroups([]);
        setSelectedGroupName(undefined);
        populateGroupData();

    }, [selectedSemesterId]);

    useEffect(() => {
        if (auth?.email && groups.length > 0) {
            const group = groups.find(g => g.id === auth.group);
            if (group) {
                setSelectedGroupName(group.name);
            }
        }
    }, [auth, groups]);

    const content = (
        <Container>
            {auth?.email ? 
                <div className='auth-buttons'>
                    <div className="flexGrow">
                        <button onClick={() => setAuth({})}>Sign Out</button>
                        <button onClick={() => navigate("/profile")}>Profile</button>
                        <h4>{auth?.email} | {groups.find(g => g.id === auth?.group)?.name}</h4>
                    </div>
                </div>
                :
                <div className='auth-buttons'>
                    <div className="flexGrow">
                        <button onClick={() => navigate("/login", { state: { from: location } })}>Log in</button>
                        <button onClick={() => navigate("/register", { state: { from: location } })}>Register</button>
                    </div>
                </div>
            }
            <div className='filters'>
                <ToggleButtonGroup
                    color="primary"
                    value={weekType}
                    exclusive
                    onChange={handleChangeWeekType}
                    aria-label="Platform"
                >
                    <ToggleButton value={WeekType.Odd}>Odd</ToggleButton>
                    <ToggleButton value={WeekType.Even}>Even</ToggleButton>
                </ToggleButtonGroup>

                <Autocomplete
                    id="async-select-semester"
                    sx={{ width: 220 }}
                    defaultValue={semesters.find(sem => sem.id === Number(selectedSemesterId))
                        ?? semesters.find(sem => sem.currentSemester)}
                    open={openS}
                    onOpen={() => setOpenS(true)}
                    onClose={() => setOpenS(false)}
                    onChange={(e, newValue) => setSelectedSemesterId(newValue ? newValue.id : undefined)}
                    isOptionEqualToValue={(option, value) => option.id === value.id}
                    getOptionLabel={(option) => (option.description ? option.description : '')}
                    options={semesters}
                    renderInput={(params) => (
                        <TextField
                            {...params}
                            label="Semesters"
                            inputProps={{
                                ...params.inputProps,
                                endadornment: (
                                    <>
                                        {params.inputProps.endadornment}
                                    </>
                                )
                            }}
                        />
                    )}
                />

                <Autocomplete
                    id="async-select-group"
                    sx={{ width: 150 }}
                    defaultValue={selectedGroupName}
                    open={openG}
                    onOpen={() => setOpenG(true)}
                    onClose={() => setOpenG(false)}
                    onChange={(e, newValue) => setSelectedGroupName(newValue ? newValue.name : '')}
                    isOptionEqualToValue={(option, value) => option.id === value.id}
                    getOptionLabel={(option) => (option.name ? option.name : '')}
                    options={groups}
                    renderInput={(params) => (
                        <TextField
                            {...params}
                            label="Groups"
                            inputProps={{
                                ...params.inputProps,
                                endadornment: (
                                    <>
                                        {params.inputProps.endadornment}
                                    </>
                                )
                            }}
                        />
                    )}
                />
            </div>
            <ScheduleTable 
                semesterId={selectedSemesterId} 
                weekType={weekType} 
                currentWeekType={currentWeekType}
                selectedGroupId={selectedGroupName} 
            />
        </Container>
    )

    return (semesters.length || groups.length)
        ? content
        : <CircularProgress />;

    async function populateSemesterData() {
        try {
            const response: AxiosResponse = await axios.get(getSemestersReq);
            const responseData: Semester[] = response.data.semesters;
            setSemesters(responseData);

            const currentSemester = responseData
                .find(semester => semester.currentSemester);
            if (currentSemester) {
                setSelectedSemesterId(currentSemester.id);
                setCurrentWeekType(response.data.weekType as WeekType);
                setWeekType(response.data.weekType as WeekType);
            }
        } catch (err: any) {
            if (isAxiosError(err)) {
                // Handle Axios-specific errors
                console.error("Axios error:", err.message);
            } else {
                // Handle general errors
                console.error("General error:", err.message);
            }

        }
    }

    async function populateGroupData() {
        if (!selectedSemesterId) {
            return;
        }

        try {
            const response: AxiosResponse = 
                await axios.get(getGroupsBySemesterIdReq(selectedSemesterId));
            const responseData: Group[] = response.data;
            setGroups(responseData);

        } catch (err: any) {
            if (isAxiosError(err)) {
                // Handle Axios-specific errors
                console.error("Axios error:", err.message);
            } else {
                // Handle general errors
                console.error("General error:", err.message);
            }

        }

    }


    function handleChangeWeekType(e: any, newWeekType: WeekType) {
        setWeekType(newWeekType);
    }
}