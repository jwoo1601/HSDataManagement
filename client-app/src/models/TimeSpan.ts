export default class TimeSpan {
  hours?: number;
  minutes?: number;
  seconds?: number;

  constructor(formatString: string) {
    console.log("NEW TIME SPAN");

    const matches = formatString.match(/(\d{2}):(\d{2}):(\d{2})/);
    if (!matches || matches.length < 2) {
      throw new Error(`Invalid TimeSpan format string: ${formatString}`);
    }

    this.hours = parseInt(matches[1]);
    if (isNaN(this.hours)) {
      this.hours = undefined;
    }

    this.minutes = parseInt(matches[2]);
    if (isNaN(this.minutes)) {
      this.minutes = undefined;
    }

    this.seconds = parseInt(matches[3]);
    if (isNaN(this.seconds)) {
      this.seconds = undefined;
    }
  }

  get totalHours() {
    return (
      (this.hours ?? 0) +
      Math.floor((this.minutes ?? 0) / 60) +
      Math.floor((this.seconds ?? 0) / 3600)
    );
  }

  get totalMinutes() {
    return (
      (this.hours ?? 0) * 60 +
      (this.minutes ?? 0) +
      Math.floor((this.seconds ?? 0) / 60)
    );
  }

  get totalSeconds() {
    return (
      (this.hours ?? 0) * 3600 + (this.minutes ?? 0) * 60 + (this.seconds ?? 0)
    );
  }

  toString() {
    return `${TimeSpan.padZero(this.hours ?? 0)}:${TimeSpan.padZero(
      this.minutes ?? 0
    )}:${TimeSpan.padZero(this.seconds ?? 0)}`;
  }

  private static padZero(n: number) {
    return String(n).padStart(2, "0");
  }
}
