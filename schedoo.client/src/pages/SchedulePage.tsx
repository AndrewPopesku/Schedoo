import { createContext, useEffect, useState } from "react";
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
import { NavBar } from "../components/NavBar.tsx";

export const IsCurrentWeekContext = createContext(false);
export const CanEditAttendanceContext = createContext(false);

export function SchedulePage() {
    const { auth } = useAuth();

    const [weekType, setWeekType] = useState<WeekType>();
    const [currentWeekType, setCurrentWeekType] = useState<WeekType>();
    const [isCurrentWeekType, setIsCurrentWeekType] = useState<boolean>(true);

    const [groups, setGroups] = useState<Group[]>([]);
    const [selectedGroupName, setSelectedGroupName] = useQueryState("groupName");
    const [openG, setOpenG] = useState<boolean>(false);

    const [semesters, setSemesters] = useState<Semester[]>([]);
    const [selectedSemesterId, setSelectedSemesterId] = useQueryState("semesterId");
    const [openS, setOpenS] = useState<boolean>(false);

    const [canEditAttendance, setCanEditAttendance] = useState<boolean>(false);

    useEffect(() => {
        populateSemesterData();
    }, []);

    useEffect(() => {
        setGroups([]);
        setSelectedGroupName(undefined);
        populateGroupData();
    }, [selectedSemesterId]);

    useEffect(() => {
        setIndividualOptions();
    }, [auth, groups]);

    function setIndividualOptions() {
        if (auth?.email && groups.length > 0) {
            const group = groups.find(g => g.id === auth.groupId);

            if (group) {
                setSelectedGroupName(group.name);
            }
            if (auth.roles.includes("Group Leader")) {
                setCanEditAttendance(true);
            } else {
                setCanEditAttendance(false);
            }
        }
    }

    const content = (
        <>
            <NavBar/>
            <Container>
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
                        value={groups.find(group => group.name === selectedGroupName) || null}
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
                <CanEditAttendanceContext.Provider value={canEditAttendance}>
                    <IsCurrentWeekContext.Provider value={isCurrentWeekType}>
                        <ScheduleTable
                            semesterId={selectedSemesterId}
                            weekType={weekType}
                            selectedGroupId={selectedGroupName}
                        />
                    </IsCurrentWeekContext.Provider>
                </CanEditAttendanceContext.Provider>
            </Container>
        </>
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
            setIndividualOptions();
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
        setIsCurrentWeekType(currentWeekType === newWeekType);
    }
}