export function clone<TModel>(value: TModel): TModel {
  if (value) {
    return JSON.parse(JSON.stringify(value)) as TModel
  } else {
    return value
  }
}
