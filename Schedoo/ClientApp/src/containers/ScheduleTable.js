import React, { useEffect, useState } from 'react';
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';
import axios from 'axios';
import ScheduleRow from '../components/ScheduleRow';

const ScheduleTable = (props) => {
  const [scheduleData, setScheduleData] = useState([]);
  const [isEmptySchedule, setIsEmptySchedule] = useState(true);
  const [timeSlots, setTimeSlots] = useState([]);
  const [days, setDays] = useState([]);
  const [currentDate, setCurrentDate] = useState(new Date());
  const currentDay = days[currentDate.getDay() - 1];

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentDate(new Date());
    }, 5000);

    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    if (props.selectedGroupId) {
      axios.get('http://fmi-schedule.chnu.edu.ua/schedules/full/groups?semesterId='
        + props.semesterId + '&groupId=' + props.selectedGroupId)
        .then(response => {
          if (response.data.schedule.length) {
            setScheduleData(response.data.schedule[0].days);
            setTimeSlots(response.data.semester.semester_classes);
            setDays(response.data.semester.semester_days);
            setIsEmptySchedule(false);
          } else {
            setIsEmptySchedule(true);
          }
        })
        .catch(error => console.error("Error fetching schedule data", error));
    } else {
      setIsEmptySchedule(true);
    }
  }, [props.selectedGroupId]);

  function blinkingDotForCurrentDay(day) {
    return (
      <div className="active-day">
        <FiberManualRecordIcon color="primary" className="blinking-dot"/>
        {day}
      </div>
    )
  }


  if (isEmptySchedule) {
    return <span>Empty schedule</span>
  }
  return (
    <table className="table table-bordered">
      <thead>
        <tr>
          <th></th>
          {days.map((day, index) => (
            <th
              key={index}
            >{day === currentDay ? blinkingDotForCurrentDay(day) : day}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {timeSlots.map(timeslot => {
          const data = { scheduleData, weekType: props.weekType, timeslot, days, currentDate }
          return <ScheduleRow key={timeslot.id} data={data} />
        })}
      </tbody>
    </table>
  )
}

export default ScheduleTable;
