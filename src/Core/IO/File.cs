/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Extensions;

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StardustSandbox.Core.IO
{
    internal static class File
    {
        private static string GetFileName(string prefix, string extension)
        {
            return string.Concat(GameConstants.ID, "_", prefix, "_", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), extension).ToLowerInvariant();
        }

        internal static string WriteRenderTarget2D(RenderTarget2D value)
        {
            _ = System.IO.Directory.CreateDirectory(Directory.Screenshots);

            string screenshotFilePath = Path.Combine(Directory.Screenshots, GetFileName("screenshot", ".png"));

            value.FlattenAlpha();

            using FileStream fs = new(screenshotFilePath, FileMode.Create, FileAccess.Write);
            value.SaveAsPng(fs, value.Width, value.Height);

            return screenshotFilePath;
        }

        internal static string WriteException(Exception value)
        {
            ArgumentNullException.ThrowIfNull(value);

            _ = System.IO.Directory.CreateDirectory(Directory.Logs);

            DateTime nowLocal = DateTime.Now;
            DateTime nowUtc = nowLocal.ToUniversalTime();
            string logFilePath = Path.Combine(Directory.Logs, GetFileName("log", ".txt"));

            StringBuilder sb = new(32_768);

            // Top-level "how to read" and summary
            _ = sb.AppendLine("############################################################");
            _ = sb.AppendLine($"  {GameConstants.TITLE} - Diagnostic Log");
            _ = sb.AppendLine($"  Generated: {nowLocal:yyyy-MM-dd HH:mm:ss.fff} (local) | {nowUtc:yyyy-MM-dd HH:mm:ss.fff}Z (UTC)");
            _ = sb.AppendLine("############################################################");
            _ = sb.AppendLine();
            _ = sb.AppendLine("GUIDE: Sections below contain environment metadata, process/runtime info, assemblies, disk,");
            _ = sb.AppendLine("       network state, threadpool/thread info, GC/memory stats and a fully expanded");
            _ = sb.AppendLine("       exception report (including AggregateException children and ex.Data entries).");
            _ = sb.AppendLine("       Sensitive environment values are redacted by default.");
            _ = sb.AppendLine();
            _ = sb.AppendLine("SUMMARY:");
            _ = sb.AppendLine($"  Exception type : {value.GetType().FullName}");
            _ = sb.AppendLine($"  Message        : {value.Message}");
            _ = sb.AppendLine($"  Primary site   : {value.TargetSite?.ToString() ?? "<unknown>"}");
            _ = sb.AppendLine();
            _ = sb.AppendLine(new string('=', 80));
            _ = sb.AppendLine();

            // Process and runtime
            AppendProcessRuntimeBlock(sb);

            // OS, time, culture, timezone
            AppendSystemBlock(sb, nowLocal, nowUtc);

            // Threadpool and threads
            AppendThreadBlock(sb);

            // Memory and GC
            AppendMemoryBlock(sb);

            // Drives / disk
            AppendDiskBlock(sb);

            // Loaded assemblies with file version and informational version if available
            AppendAssembliesBlock(sb);

            // Exception details (expands inner chains, AggregateException, ex.Data)
            _ = sb.AppendLine(new string('-', 80));
            _ = sb.AppendLine("DETAILED EXCEPTION REPORT:");
            _ = sb.AppendLine(new string('-', 80));
            _ = sb.AppendLine(FormatExceptionDetailed(value));

            // Write file
            System.IO.File.WriteAllText(logFilePath, sb.ToString(), Encoding.UTF8);

            return logFilePath;
        }

        #region Blocks

        private static void AppendProcessRuntimeBlock(StringBuilder sb)
        {
            try
            {
                using Process proc = Process.GetCurrentProcess();
                _ = sb.AppendLine("PROCESS / RUNTIME");
                _ = sb.AppendLine(new string('-', 60));
                _ = sb.AppendLine($"Process name       : {proc.ProcessName}");
                _ = sb.AppendLine($"PID                : {proc.Id}");
                _ = sb.AppendLine($"Started            : {SafeGet(() => proc.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), "<n/a>")}");
                _ = sb.AppendLine($"Total CPU (proc)   : {SafeGet(proc.TotalProcessorTime.ToString, "<n/a>")}");
                _ = sb.AppendLine($"Threads (count)    : {proc.Threads.Count}");
                _ = sb.AppendLine($"Handle count       : {SafeGet(proc.HandleCount.ToString, "<n/a>")}");
                _ = sb.AppendLine($"Working set        : {proc.WorkingSet64:N0} bytes");
                _ = sb.AppendLine($"Private memory     : {proc.PrivateMemorySize64:N0} bytes");
                _ = sb.AppendLine($"User               : {Environment.UserName} @ {Environment.MachineName}");
                _ = sb.AppendLine($"Command line       : {SafeGet(() => Environment.CommandLine, "<n/a>")}");
                _ = sb.AppendLine($".NET runtime        : {RuntimeInformation.FrameworkDescription} ({RuntimeInformation.ProcessArchitecture})");
                _ = sb.AppendLine($"CLR version        : {Environment.Version}");
                _ = sb.AppendLine();
            }
            catch (Exception ex)
            {
                _ = sb.AppendLine("PROCESS / RUNTIME: failed to collect process information: " + ex);
            }
        }

        private static void AppendSystemBlock(StringBuilder sb, DateTime nowLocal, DateTime nowUtc)
        {
            try
            {
                _ = sb.AppendLine("SYSTEM");
                _ = sb.AppendLine(new string('-', 60));
                _ = sb.AppendLine($"OS                  : {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})");
                _ = sb.AppendLine($"Machine uptime      : {FormatTimeSpanFromMs(Environment.TickCount64)} (Environment.TickCount64 ms)");
                _ = sb.AppendLine($"Local timestamp     : {nowLocal:yyyy-MM-dd HH:mm:ss.fff}");
                _ = sb.AppendLine($"UTC timestamp       : {nowUtc:yyyy-MM-dd HH:mm:ss.fff}Z");
                _ = sb.AppendLine($"Time zone           : {TimeZoneInfo.Local.StandardName} ({TimeZoneInfo.Local.Id})");
                _ = sb.AppendLine($"Culture             : {CultureInfo.CurrentCulture.Name} (UI: {CultureInfo.CurrentUICulture.Name})");
                _ = sb.AppendLine();
            }
            catch (Exception ex)
            {
                _ = sb.AppendLine("SYSTEM: failed to collect system information: " + ex);
            }
        }

        private static void AppendThreadBlock(StringBuilder sb)
        {
            try
            {
                _ = sb.AppendLine("THREADPOOL / THREADS");
                _ = sb.AppendLine(new string('-', 60));

                ThreadPool.GetAvailableThreads(out int workerAvailable, out int ioAvailable);
                ThreadPool.GetMaxThreads(out int workerMax, out int ioMax);
                ThreadPool.GetMinThreads(out int workerMin, out int ioMin);

                _ = sb.AppendLine($"ThreadPool available workers : {workerAvailable}");
                _ = sb.AppendLine($"ThreadPool available IO      : {ioAvailable}");
                _ = sb.AppendLine($"ThreadPool max workers       : {workerMax}");
                _ = sb.AppendLine($"ThreadPool max IO            : {ioMax}");
                _ = sb.AppendLine($"ThreadPool min workers       : {workerMin}");
                _ = sb.AppendLine($"ThreadPool min IO            : {ioMin}");
                _ = sb.AppendLine($"Managed thread id            : {Environment.CurrentManagedThreadId}");
                _ = sb.AppendLine();
            }
            catch (Exception ex)
            {
                _ = sb.AppendLine("THREADPOOL / THREADS: failed to collect thread info: " + ex);
            }
        }

        private static void AppendMemoryBlock(StringBuilder sb)
        {
            try
            {
                _ = sb.AppendLine("MEMORY / GC");
                _ = sb.AppendLine(new string('-', 60));
                _ = sb.AppendLine($"GC total memory       : {GC.GetTotalMemory(false):N0} bytes");
                _ = sb.AppendLine($"GC collections        : gen0={GC.CollectionCount(0)} gen1={GC.CollectionCount(1)} gen2={GC.CollectionCount(2)}");
                _ = sb.AppendLine($"GC settings           : Server GC={(GetIsServerGC() ? "Yes" : "No")}, LatencyMode={GCSettings.LatencyMode}");
                _ = sb.AppendLine();
            }
            catch (Exception ex)
            {
                _ = sb.AppendLine("MEMORY / GC: failed to collect GC info: " + ex);
            }
        }

        private static void AppendDiskBlock(StringBuilder sb)
        {
            try
            {
                _ = sb.AppendLine("DISK / DRIVES");
                _ = sb.AppendLine(new string('-', 60));
                foreach (DriveInfo d in DriveInfo.GetDrives().Where(d => d.IsReady).OrderBy(d => d.Name))
                {
                    _ = sb.AppendLine($"Drive {d.Name} - Type: {d.DriveType}, FS: {d.DriveFormat}, Total: {d.TotalSize:N0} bytes, Free: {d.AvailableFreeSpace:N0} bytes");
                }

                _ = sb.AppendLine();
            }
            catch (Exception ex)
            {
                _ = sb.AppendLine("DISK / DRIVES: failed to collect drive info: " + ex);
            }
        }

        private static void AppendAssembliesBlock(StringBuilder sb)
        {
            try
            {
                _ = sb.AppendLine("LOADED ASSEMBLIES");
                _ = sb.AppendLine(new string('-', 60));
                IOrderedEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .OrderBy(a => a.GetName().Name, StringComparer.OrdinalIgnoreCase);

                foreach (Assembly asm in assemblies)
                {
                    try
                    {
                        AssemblyName name = asm.GetName();
                        string location = SafeGet(() => asm.Location, "<dynamic>");
                        string fileVer = "<n/a>";
                        string infoVer = "<n/a>";

                        if (!string.IsNullOrEmpty(location) && System.IO.File.Exists(location))
                        {
                            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(location);
                            fileVer = fvi?.FileVersion ?? "<n/a>";
                            infoVer = fvi?.ProductVersion ?? infoVer;
                        }
                        else
                        {
                            // try assembly attributes
                            AssemblyInformationalVersionAttribute aiv = asm.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
                                         .OfType<AssemblyInformationalVersionAttribute>()
                                         .FirstOrDefault();
                            if (aiv != null)
                            {
                                infoVer = aiv.InformationalVersion;
                            }
                        }

                        _ = sb.AppendLine($" - {name.Name}, Version={name.Version}, Location={location}");
                        _ = sb.AppendLine($"   FileVersion={fileVer}, InformationalVersion={infoVer}");
                    }
                    catch (Exception aex)
                    {
                        _ = sb.AppendLine($" - {SafeGet(() => asm.GetName().Name, "<unknown>")} (failed to read assembly details: {aex.Message})");
                    }
                }

                _ = sb.AppendLine();
            }
            catch (Exception ex)
            {
                _ = sb.AppendLine("LOADED ASSEMBLIES: failed to collect assemblies: " + ex);
            }
        }

        #endregion

        #region Helpers

        private static string FormatExceptionDetailed(Exception ex)
        {
            StringWriter sw = new();

            void WriteOne(Exception e, int level)
            {
                if (e is null)
                {
                    return;
                }

                string indent = new(' ', level * 2);
                sw.WriteLine($"{indent}Exception level {level}: {e.GetType().FullName}");
                sw.WriteLine($"{indent}Message       : {e.Message}");
                sw.WriteLine($"{indent}HResult       : 0x{e.HResult:X8}");
                sw.WriteLine($"{indent}Source        : {e.Source ?? "<null>"}");
                sw.WriteLine($"{indent}TargetSite    : {e.TargetSite?.ToString() ?? "<null>"}");
                sw.WriteLine($"{indent}Stack trace:");
                try
                {
                    StackTrace st = new(e, true);
                    if (st.FrameCount == 0)
                    {
                        sw.WriteLine($"{indent}  <no frames>");
                    }
                    else
                    {
                        for (int i = 0; i < st.FrameCount; i++)
                        {
                            StackFrame f = st.GetFrame(i);
                            MethodBase method = f.GetMethod();
                            string methodStr = method?.DeclaringType != null
                                ? $"{method.DeclaringType.FullName}.{method.Name}"
                                : method?.Name ?? "<unknown method>";

                            string file = f.GetFileName();
                            int line = f.GetFileLineNumber();
                            int col = f.GetFileColumnNumber();

                            if (!string.IsNullOrEmpty(file))
                            {
                                sw.WriteLine($"{indent}  at {methodStr} in {file}:line {line},col {col}");
                            }
                            else
                            {
                                sw.WriteLine($"{indent}  at {methodStr} (IL offset: {f.GetILOffset()})");
                            }
                        }
                    }
                }
                catch (Exception stex)
                {
                    sw.WriteLine($"{indent}  <failed to enumerate frames: {stex.Message}>");
                    sw.WriteLine($"{indent}  raw stacktrace: {e.StackTrace ?? "<none>"}");
                }

                // Data dictionary
                if (e.Data != null && e.Data.Count > 0)
                {
                    sw.WriteLine($"{indent}Data:");
                    foreach (object key in e.Data.Keys)
                    {
                        try
                        {
                            object val = e.Data[key];
                            string sval = val == null ? "<null>" : val.ToString()!;
                            sw.WriteLine($"{indent}  {key} = {Truncate(sval, 1000)}");
                        }
                        catch
                        {
                            sw.WriteLine($"{indent}  {key} = <unreadable>");
                        }
                    }
                }

                sw.WriteLine();

                if (e is AggregateException agg && agg.InnerExceptions?.Count > 0)
                {
                    int i = 0;
                    foreach (Exception inner in agg.InnerExceptions)
                    {
                        sw.WriteLine($"{indent}--- Aggregate inner #{i} ---");
                        WriteOne(inner, level + 1);
                        i++;
                    }
                }
                else if (e.InnerException != null)
                {
                    sw.WriteLine($"{indent}--- Inner exception ---");
                    WriteOne(e.InnerException, level + 1);
                }
            }

            WriteOne(ex, 0);
            return sw.ToString();
        }

        private static string Truncate(string value, int max)
        {
            return value is null ? "<null>" : value.Length <= max ? value : value[..max] + "...[truncated]";
        }

        private static string SafeGet(Func<string> f, string fallback)
        {
            try
            { return f(); }
            catch { return fallback; }
        }

        private static string FormatTimeSpanFromMs(long ms)
        {
            try
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(ms);
                return $"{(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
            }
            catch { return "<n/a>"; }
        }

        private static bool GetIsServerGC()
        {
            try
            {
                // available in System.Runtime
                return System.Runtime.GCSettings.IsServerGC;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
