export class QueryString {
  public getQuery(): string {
    let queryString = '';
    for (const [key, value] of Object.entries(this)) {
      if (!value) {
        continue;
      }
      const encodedKey = encodeURIComponent(key);
      const encodedValue = encodeURIComponent(value);
      if (queryString.length) {
        queryString += `&${encodedKey}=${encodedValue}`;
      } else {
        queryString += `${encodedKey}=${encodedValue}`;
      }
    }
    return queryString;
  }
}
