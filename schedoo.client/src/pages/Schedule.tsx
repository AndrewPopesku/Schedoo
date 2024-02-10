import {useEffect, useState } from "react";
import {WeekType} from "../types/enums.ts";
import {Group, Semester} from "../types/interfaces.ts";
import {getCurrentWeekType} from "../types/helpers.ts";
import {Autocomplete, Container, TextField, ToggleButton, ToggleButtonGroup } from "@mui/material";
import {ScheduleTable} from "../containers/ScheduleTable.tsx";
import '../styles/Schedule.css';

export function Schedule() {
    const [weekType, setWeekType] = useState<WeekType>(WeekType.Odd);

    const [groups, setGroups] = useState<Group[]>([]);
    const [selectedGroupName, setSelectedGroupName] = useState<string>();
    const [openG, setOpenG] = useState<boolean>(false);

    const [semesters, setSemesters] = useState<Semester[]>([]);
    const [selectedSemesterId, setSelectedSemesterId] = useState<number>();
    const [openS, setOpenS] = useState<boolean>(false);

    // const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        populateSemesterData();
    }, []);

    useEffect(() => {
        setGroups([]);
        setSelectedGroupName(undefined);
        populateGroupData();
    }, [selectedSemesterId]);

    const content = (
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
                    sx={{width: 220}}
                    defaultValue={semesters.find(sem => sem.currentSemester)}
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
                    sx={{width: 150}}
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
            <ScheduleTable semesterId={selectedSemesterId} weekType={weekType} selectedGroupId={selectedGroupName}/>
        </Container>
    )

    return (semesters.length || groups.length) ? content : "nothing";
    
    async function populateSemesterData() {
        await fetch('getsemesters')
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`)
                }

                return response.json();
            })
            .then((data: Semester[]) => {
                setSemesters(data);

                const currentSemester = data
                    .find(semester => semester.currentSemester);
                if (currentSemester) {
                    setSelectedSemesterId(currentSemester.id);
                    setWeekType(getCurrentWeekType(currentSemester));
                }
            });
    }

    async function populateGroupData() {
        if (!selectedSemesterId) {
            // You can return early or handle it based on your requirements
            console.error("Selected semester ID is undefined");
            return;
        }

        await fetch('getgroups/semesterId=' + selectedSemesterId)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`)
                }

                return response.json();
            })
            .then((data: Group[]) => {
                setGroups(data);
            });

    }
    
    function handleChangeWeekType(event: any, newWeekType: WeekType) {
        setWeekType(newWeekType);
    }
}

