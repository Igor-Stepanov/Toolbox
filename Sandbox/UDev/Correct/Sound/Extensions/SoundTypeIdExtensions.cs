namespace Sandbox.UDev.Correct.Sound.Extensions
{
  public static class SoundTypeIdExtensions
  {
    public static void Play(this SoundTypeId self) =>
      Sounds.Play(self);
  }
}