// using System;
// using System.IO;
using System.Runtime.InteropServices;

namespace function;

public class EnvironmentInfo
{
    public EnvironmentInfo()
    {
        RuntimeVersion = RuntimeInformation.FrameworkDescription;
        OSVersion = RuntimeInformation.OSDescription;
        OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
        ProcessorCount = Environment.ProcessorCount;
        GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
        TotalAvailableMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

        var hasCgroup = RuntimeInformation.OSDescription.StartsWith("Linux") && Directory.Exists("/sys/fs/cgroup/memory");
        if (!hasCgroup) return;

        var limit = File.ReadAllLines("/sys/fs/cgroup/memory/memory.limit_in_bytes")[0];
        var usage = File.ReadAllLines("/sys/fs/cgroup/memory/memory.usage_in_bytes")[0];
        MemoryLimit = long.Parse(limit);
        MemoryUsage = long.Parse(usage);
    }

    public string RuntimeVersion { get; set; }
    public string OSVersion { get; set; }
    public string OSArchitecture { get; set; }
    public int ProcessorCount { get; set; }
    public long TotalAvailableMemoryBytes {get; set;}
    public long MemoryLimit {get; set;}
    public long MemoryUsage {get; set;}

}
