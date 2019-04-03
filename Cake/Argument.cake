string ArgumentOrThrow(string argument)
{
  string value = Argument<string>(argument, null);

  if (string.IsNullOrWhiteSpace(value))
    throw new Exception($"Argument '{argument}' was not supplied to Cake script.");

  return value;
}
