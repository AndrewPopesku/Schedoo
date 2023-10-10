import React, { useState, useEffect } from 'react';
import { isInTimeSlot, weekTypes } from '../helpers';
import ScheduleCell from './ScheduleCell';

const ScheduleRow = ({ data }) => {
    return (
        <tr>
            <td className="timeslot-cell">
                <div>
                    <p>{data.timeslot.startTime}</p>
                    <div className='line'></div>
                    <p>{data.timeslot.endTime}</p>
                </div>
            </td>
            { data.days.map(day => {
                const dayClass = data.scheduleData
                    .find(d => d.day === day)
                    .classes.find(c => c.class.id === data.timeslot.id);
                
                var classData;
                if (dayClass) {
                    classData = (data.weekType === weekTypes.odd) ? dayClass.weeks.odd : dayClass.weeks.even;
                }

                const isActiveClass = (data.days[data.currentDate.getDay() - 1] === day)
                    && isInTimeSlot(data.currentDate, data.timeslot);

                return <ScheduleCell key={day} isActive={isActiveClass} classData={classData} />;
            })}
        </tr>
    );
};

export default ScheduleRow;
