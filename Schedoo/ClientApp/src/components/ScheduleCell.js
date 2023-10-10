import React from 'react'

const ScheduleCell = ({ isActive, classData }) => {
  return (
    <td className={isActive ? 'active-card' : ''}>
      {classData &&
        <div className="class-card">
          <div className="class-content">
            <div className="class-header">
              <p className="class-title">{classData.subjectForSite}</p>
              <i>{classData.teacher.name} {classData.teacher.surname}</i>
            </div>
            <div className="class-info">
              <p className='class-type'>#{classData.lessonType.toLowerCase()}</p>
              <p className="class-room">{classData.room.name}</p>
            </div>
          </div>
        </div>
      }
    </td>
  )
}

export default ScheduleCell