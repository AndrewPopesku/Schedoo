import { AttendanceStatus } from '../types/interfaces';

type AttendanceStatusType = keyof typeof AttendanceStatus;

export function Select({ label, value, values, onChange }
    : {
        label: string,
        value: AttendanceStatusType,
        values: AttendanceStatusType[],
        onChange: (value: AttendanceStatusType) => void
    }) {
    return (
        <div>
            <label>{label}</label>
            <select
                title={label}
                value={value}
                onChange={(e) => onChange(e.target.value as AttendanceStatusType)}
            >
                {values.map((val) => (
                    <option key={val} value={val}>
                        {val}
                    </option>
                ))}
            </select>
        </div>
    );
};