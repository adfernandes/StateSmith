using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace StateSmith.Cli.Run;

public class SsCsxFileFinder
{
    Matcher matcher;
    private bool hasIncludePatterns;

    public SsCsxFileFinder()
    {
        matcher = new();
    }

    public void SetAsRecursive()
    {
        matcher.AddInclude("./**/*.csx");
    }

    public void ClearIncludePatterns()
    {
        matcher = new(); // Reset the matcher
    }

    public void AddDefaultIncludePatternIfNone()
    {
        if (!hasIncludePatterns)
        {
            matcher.AddInclude("./*.csx");
        }
    }

    public void AddIncludePattern(string pathPattern)
    {
        hasIncludePatterns = true;
        matcher.AddInclude(pathPattern);
    }

    public void AddIncludePatterns(IEnumerable<string>? pathPatterns)
    {
        if (pathPatterns == null)
            return;

        foreach (var pattern in pathPatterns)
        {
            AddIncludePattern(pattern);
        }
    }

    public void AddExcludePattern(string pathPattern)
    {
        matcher.AddExclude(pathPattern);
    }

    public void AddExcludePatterns(IEnumerable<string>? pathPatterns)
    {
        if (pathPatterns == null)
            return;
        
        foreach (var pattern in pathPatterns)
        {
            AddExcludePattern(pattern);
        }
    }

    public List<string> Scan(string searchDirectory)
    {
        List<string> targetCsxFiles = new();

        PatternMatchingResult result = matcher.Execute(
            new DirectoryInfoWrapper(
                new DirectoryInfo(searchDirectory)));

        foreach (var file in result.Files)
        {
            if (IsTargetScriptFile(searchDirectory + "/" + file.Path))
            {
                targetCsxFiles.Add(file.Path);
            }
        }

        return targetCsxFiles;
    }

    internal bool IsTargetScriptFile(string path)
    {
        var text = File.ReadAllText(path);
        return IsTargetScriptContent(text);
    }

    internal static bool IsTargetScriptContent(string text)
    {
        if (text.Contains("//<statesmith.cli-ignore-this-file>"))
        {
            return false;
        }

        if (Regex.IsMatch(text, @"(?xim)
            ^ \s*
            [#]r \s* ""nuget \s*  : \s* StateSmith \s* ,"))
        {
            return true;
        }

        return false;
    }
}
