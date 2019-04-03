void InformationArray(string message, IEnumerable<object> array)
{
  Information(message);

  foreach (var item in array)
    Information(item);

  Information("");
}
