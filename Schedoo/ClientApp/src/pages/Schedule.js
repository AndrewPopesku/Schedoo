import React, { useEffect, useState } from 'react';
import ScheduleTable from '../containers/ScheduleTable';
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import TextField from '@mui/material/TextField';
import { Autocomplete, CircularProgress, Container } from '@mui/material/';
import { weekTypes, getCurrentWeekType } from '../helpers';
import axios from 'axios';
import '../styles/schedule.css';

const Schedule = () => {
  const [weekType, setWeekType] = useState('');

  const [groups, setGroups] = useState([]);
  const [selectedGroupId, setSelectedGroupId] = useState('');
  const [openG, setOpenG] = useState(false);

  const [semesters, setSemesters] = useState([]);
  const [semesterId, setSemesterId] = useState(55);
  const [openS, setOpenS] = useState(false);

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    axios.get('http://fmi-schedule.chnu.edu.ua/public/semesters')
      .then(response => {
        const semesterData = response.data.map(semester => ({
          id: semester.id,
          label: semester.description,
          startDay: semester.startDay,
          endDay: semester.endDay,
          currentSemester: semester.currentSemester
        }));
        
        setSemesters(semesterData);
  
        const currentSemester = semesterData.find(semester => semester.currentSemester);
        if (currentSemester) {
          setSemesterId(currentSemester.id);
          setWeekType(getCurrentWeekType(currentSemester));
        }
  
        setLoading(false);
      })
      .catch(error => {
        console.error("Error fetching semesters data", error);
        setLoading(false);
      });
  }, []);

  useEffect(() => {
    setGroups([]);
    setSelectedGroupId('');
    axios.get('http://fmi-schedule.chnu.edu.ua/semesters/' + semesterId + '/groups')
      .then(responce => {
        setGroups(responce.data.map(group => ({
          id: group.id,
          label: group.title
        }))
        );
      })
      .catch(error => {
        console.error("Error fetching groups data", error);
      })
  }, [semesterId])

  function handleChangeWeekType(e, newWeekType) {
    setWeekType(newWeekType);
  }


  if (loading) {
    return <CircularProgress />
  }

  return (
    <Container>
      <div className='filters'>
        <ToggleButtonGroup
          color="primary"
          value={weekType}
          exclusive
          onChange={handleChangeWeekType}
          aria-label="Platform"
        >
          <ToggleButton value={weekTypes.odd}>Odd</ToggleButton>
          <ToggleButton value={weekTypes.even}>Even</ToggleButton>
        </ToggleButtonGroup>

        <Autocomplete
          id="async-select-semester"
          sx={{ width: 220 }}
          defaultValue={semesters.find(sem => sem.currentSemester)}
          open={openS}
          onOpen={() => setOpenS(true)}
          onClose={() => setOpenS(false)}
          onChange={(e, newValue) => setSemesterId(newValue ? newValue.id : '')}
          isOptionEqualToValue={(option, value) => option.id === value.id}
          getOptionLabel={(option) => option.label}
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
          open={openG}
          onOpen={() => setOpenG(true)}
          onClose={() => setOpenG(false)}
          onChange={(e, newValue) => setSelectedGroupId(newValue ? newValue.id : '')}
          isOptionEqualToValue={(option, value) => option.id === value.id}
          getOptionLabel={(option) => option.label}
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
      <ScheduleTable semesterId={semesterId} weekType={weekType} selectedGroupId={selectedGroupId} />
    </Container>
  )
}

export default Schedule;