namespace sdmap.Macros
{
    public class Macro
    {
        public string Name { get; set; }

        public bool SkipArgumentRuntimeCheck { get; set; }

        public SdmapTypes[] Arguments { get; set; }

        public MacroDelegate Method { get; set; }
    }
}
