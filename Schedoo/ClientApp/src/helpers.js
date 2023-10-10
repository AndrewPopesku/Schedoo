export function timeStrToDate(timeStr) {
  const parts = timeStr.split(":");
  const hours = parseInt(parts[0], 10);
  const mins = parseInt(parts[1], 10);

  const tmpDate = new Date();
  tmpDate.setHours(hours);
  tmpDate.setMinutes(mins);
  tmpDate.setSeconds(0);

  return tmpDate;
}

export function isInTimeSlot(timeDate, timeslot) {
  return timeDate > timeStrToDate(timeslot.startTime) && timeDate < timeStrToDate(timeslot.endTime);
}

export function parseDate(dateString) {
  const parts = dateString.split('/');
  if (parts.length === 3) {
    const day = parseInt(parts[0], 10);
    const month = parseInt(parts[1], 10) - 1;
    const year = parseInt(parts[2], 10);
    return new Date(year, month, day);
  }
  return null;
}

export const weekTypes = {
  even: 0,
  odd: 1,
}

export function getCurrentWeekType(semester) {
  const startSemesterDate = new Date(parseDate(semester.startDay));
  const timeDiff = new Date() - startSemesterDate;
  const daysElapsed  = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
  const weeksElapsed = Math.floor(daysElapsed / 7);

  return weeksElapsed % 2 === 0 ? weekTypes.even : weekTypes.odd;
}