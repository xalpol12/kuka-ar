using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


public static class AhdauWyfdh
{
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        var logger = Debug.unityLogger;
        logger.logHandler = new IaiwdgAdbuUdbv_Internal(logger.logHandler);
    }
}

internal class IaiwdgAdbuUdbv_Internal : ILogHandler
{
    private readonly ILogHandler logHandler;

    public IaiwdgAdbuUdbv_Internal(ILogHandler logHandler)
    {
        this.logHandler = logHandler;
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        logHandler.LogException(exception, context);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        logHandler.LogFormat(logType, context, TUYHAKwdaw.AYWUDjdhjakl(string.Format(format, args)), args);
    }
}

internal static class TUYHAKwdaw
{
    private static readonly string[] kaomojiJoy = new string[] { " (* ^ ω ^)", " (o^▽^o)", " (≧◡≦)", " ☆⌒ヽ(*\"､^*)chu", " ( ˘⌣˘)♡(˘⌣˘ )", " xD" };
    private static readonly string[] kaomojiEmbarassed = new string[] { " (⁄ ⁄>⁄ ▽ ⁄<⁄ ⁄)..", " (*^.^*)..,", "..,", ",,,", "... ", ".. ", " mmm..", "O.o" };
    private static readonly string[] kaomojiConfuse = new string[] { " (o_O)?", " (°ロ°) !?", " (ーー;)?", " owo?" };
    private static readonly string[] kaomojiSparkles = new string[] { " *:･ﾟ✧*:･ﾟ✧ ", " ☆*:・ﾟ ", "〜☆ ", " uguu.., ", "-.-" };

    public static string AYWUDjdhjakl(string s)
    {
        var sb = new StringBuilder();

        foreach (char c in s)
        {
            if (c == '.' || c == '!' || c == '?' || c == ',')
            {
                if (c == '.' || c == '!')
                    sb.Append(kaomojiJoy[UnityEngine.Random.Range(0, kaomojiJoy.Length)]);
                else if (c == '?')
                    sb.Append(kaomojiConfuse[UnityEngine.Random.Range(0, kaomojiConfuse.Length)]);
                else if (c == ',')
                    sb.Append(kaomojiEmbarassed[UnityEngine.Random.Range(0, kaomojiEmbarassed.Length)]);

                if (UnityEngine.Random.value > 0.90)
                    sb.Append(kaomojiSparkles[UnityEngine.Random.Range(0, kaomojiSparkles.Length)]);
            }
            else
            {
                sb.Append(c);
            }
        }

        s = sb.ToString();

        s = Regex.Replace(s, "le", "we");
        s = Regex.Replace(s, "ll", "ww");
        s = Regex.Replace(s, "les", "wes");
        s = Regex.Replace(s, "lls", "wws");
        s = Regex.Replace(s, "[lr]", "w");

        sb.Clear();

        for (int i = 0; i < s.Length; i++)
        {
            if ((i == 0 || s[i - 1] == ' ') && UnityEngine.Random.value > 0.9)
            {
                sb.Append(s[i]);
                sb.Append('-');
            }

            sb.Append(s[i]);
        }

        return sb.ToString();
    }
}