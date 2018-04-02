﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Quaver.API.Osu
{
    public class PeppyBeatmap
    {
        /// <summary>
        ///     The original file name of the .osu
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        ///     Is the peppy beatmap valid?
        /// </summary>
        public bool IsValid { get; set; }

        public string PeppyFileFormat { get; set; }

        // [General]
        public string AudioFilename { get; set; }
        public int AudioLeadIn { get; set; }
        public int PreviewTime { get; set; }
        public int Countdown { get; set; }
        public string SampleSet { get; set; }
        public float StackLeniency { get; set; }
        public int Mode { get; set; }
        public int LetterboxInBreaks { get; set; }
        public int SpecialStyle { get; set; }
        public int WidescreenStoryboard { get; set; }

        // [Editor]
        public string Bookmarks { get; set; }
        public float DistanceSpacing { get; set; }
        public int BeatDivisor { get; set; }
        public int GridSize { get; set; }
        public float TimelineZoom { get; set; }

        // [Metadata]
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Creator { get; set; }
        public string Version { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public int BeatmapID { get; set; }
        public int BeatmapSetID { get; set; }

        // [Difficulty]
        public float HPDrainRate { get; set; }
        public int KeyCount { get; set; }
        public float OverallDifficulty { get; set; }
        public float ApproachRate { get; set; }
        public float SliderMultiplier { get; set; }
        public float SliderTickRate { get; set; }

        // [Events]
        public string Background { get; set; }

        // [TimingPoints]
        public List<TimingPoint> TimingPoints { get; set; } = new List<TimingPoint>();

        // [HitObjects]
        public List<HitObject> HitObjects { get; set; } = new List<HitObject>();

        /// <summary>
        ///     Ctor - Automatically parses a Peppy beatmap
        /// </summary>
        public PeppyBeatmap(string filePath)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            if (!File.Exists(filePath.Trim()))
            {
                IsValid = false;
                return;
            }

            // Create a new beatmap object and default the validity to true.
            IsValid = true;
            OriginalFileName = filePath;

            // This will hold the section of the beatmap that we are parsing.
            var section = "";

            try
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    switch (line.Trim())
                    {
                        case "[General]":
                            section = "[General]";
                            break;
                        case "[Editor]":
                            section = "[Editor]";
                            break;
                        case "[Metadata]":
                            section = "[Metadata]";
                            break;
                        case "[Difficulty]":
                            section = "[Difficulty]";
                            break;
                        case "[Events]":
                            section = "[Events]";
                            break;
                        case "[TimingPoints]":
                            section = "[TimingPoints]";
                            break;
                        case "[HitObjects]":
                            section = "[HitObjects]";
                            break;
                        case "[Colours]":
                            section = "[Colours]";
                            break;
                        default:
                            break;
                    }

                    // Parse Peppy file format
                    if (line.StartsWith("osu file format"))
                        PeppyFileFormat = line;

                    // Parse [General] Section
                    if (section.Equals("[General]"))
                    {
                        if (line.Contains(":"))
                        {
                            var key = line.Substring(0, line.IndexOf(':'));
                            var value = line.Split(':').Last().Trim();

                            switch (key.Trim())
                            {
                                case "AudioFilename":
                                    AudioFilename = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "AudioLeadIn":
                                    AudioLeadIn = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "PreviewTime":
                                    PreviewTime = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "Countdown":
                                    Countdown = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "SampleSet":
                                    SampleSet = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value)); ;
                                    break;
                                case "StackLeniency":
                                    StackLeniency = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "Mode":
                                    Mode = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    if (Mode != 3)
                                        IsValid = false;
                                    break;
                                case "LetterboxInBreaks":
                                    LetterboxInBreaks = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "SpecialStyle":
                                    SpecialStyle = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "WidescreenStoryboard":
                                    WidescreenStoryboard = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;

                            }

                        }

                    }

                    // Parse [Editor] Data
                    if (section.Equals("[Editor]"))
                    {
                        if (line.Contains(":"))
                        {
                            var key = line.Substring(0, line.IndexOf(':'));
                            var value = line.Split(':').Last();

                            switch (key.Trim())
                            {
                                case "Bookmarks":
                                    Bookmarks = value;
                                    break;
                                case "DistanceSpacing":
                                    DistanceSpacing = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "BeatDivisor":
                                    BeatDivisor = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "GridSize":
                                    GridSize = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "TimelineZoom":
                                    TimelineZoom = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                            }
                        }

                    }

                    // Parse [Editor] Data
                    if (section.Equals("[Metadata]"))
                    {
                        if (line.Contains(":"))
                        {
                            var key = line.Substring(0, line.IndexOf(':'));
                            var value = line.Split(':').Last();

                            switch (key.Trim())
                            {
                                case "Title":
                                    Title = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "TitleUnicode":
                                    TitleUnicode = value;
                                    break;
                                case "Artist":
                                    Artist = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "ArtistUnicode":
                                    ArtistUnicode = value;
                                    break;
                                case "Creator":
                                    Creator = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "Version":
                                    Version = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "Source":
                                    Source = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "Tags":
                                    Tags = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                                    break;
                                case "BeatmapID":
                                    BeatmapID = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "BeatmapSetID":
                                    BeatmapSetID = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                default:
                                    break;
                            }
                        }

                    }

                    // Parse [Difficulty] Data
                    if (section.Equals("[Difficulty]"))
                    {
                        if (line.Contains(":"))
                        {
                            var key = line.Substring(0, line.IndexOf(':'));
                            var value = line.Split(':').Last();

                            switch (key.Trim())
                            {
                                case "HPDrainRate":
                                    HPDrainRate = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "CircleSize":
                                    KeyCount = Int32.Parse(value, CultureInfo.InvariantCulture);
                                    if (KeyCount != 4 && KeyCount != 7)
                                        IsValid = false;
                                    break;
                                case "OverallDifficulty":
                                    OverallDifficulty = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "ApproachRate":
                                    ApproachRate = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "SliderMultiplier":
                                    SliderMultiplier = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                                case "SliderTickRate":
                                    SliderTickRate = float.Parse(value, CultureInfo.InvariantCulture);
                                    break;
                            }
                        }

                    }

                    // Parse [Events] Data
                    if (section.Equals("[Events]"))
                    {
                        // We only care about parsing the background path,
                        // So there's no need to parse the storyboard data.
                        if (line.Contains("png") || line.Contains("jpg") || line.Contains("jpeg"))
                        {
                            string[] values = line.Split(',');
                            Background = values[2].Replace("\"", "");
                        }
                    }

                    try
                    {
                        // Parse [TimingPoints] Data
                        if (section.Equals("[TimingPoints]"))
                        {
                            if (line.Contains(","))
                            {
                                string[] values = line.Split(',');

                                var timingPoint = new TimingPoint();

                                timingPoint.Offset = float.Parse(values[0], CultureInfo.InvariantCulture);
                                timingPoint.MillisecondsPerBeat = float.Parse(values[1], CultureInfo.InvariantCulture);
                                timingPoint.Meter = Int32.Parse(values[2], CultureInfo.InvariantCulture);
                                timingPoint.SampleType = Int32.Parse(values[3], CultureInfo.InvariantCulture);
                                timingPoint.SampleSet = Int32.Parse(values[4], CultureInfo.InvariantCulture);
                                timingPoint.Volume = Int32.Parse(values[5], CultureInfo.InvariantCulture);
                                timingPoint.Inherited = Int32.Parse(values[6], CultureInfo.InvariantCulture);
                                timingPoint.KiaiMode = Int32.Parse(values[7], CultureInfo.InvariantCulture);

                                TimingPoints.Add(timingPoint);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        IsValid = false;
                    }

                    // Parse [HitObjects] Data
                    if (section.Equals("[HitObjects]"))
                    {
                        if (line.Contains(","))
                        {
                            if (line.Contains("P") || line.Contains("L") || line.Contains("|"))
                                continue;

                            string[] values = line.Split(',');

                            // We'll need to parse LNs differently than normal HitObjects,
                            // as they have a different syntax. 128 in the object's type
                            // signifies that it is an LN
                            HitObject hitObject = new HitObject();

                            hitObject.X = Int32.Parse(values[0], CultureInfo.InvariantCulture);

                            // 4k and 7k have both different hit object parsing.
                            if (KeyCount == 4)
                            {
                                if (hitObject.X >= 0 && hitObject.X <= 127)
                                    hitObject.Key1 = true;
                                else if (hitObject.X >= 128 && hitObject.X <= 255)
                                    hitObject.Key2 = true;
                                else if (hitObject.X >= 256 && hitObject.X <= 383)
                                    hitObject.Key3 = true;

                                else if (hitObject.X >= 384 && hitObject.X <= 511)
                                    hitObject.Key4 = true;
                            }
                            // 7k
                            else if (KeyCount == 7)
                            {
                                if (hitObject.X >= 0 && hitObject.X <= 108)
                                    hitObject.Key1 = true;
                                else if (hitObject.X >= 109 && hitObject.X <= 181)
                                    hitObject.Key2 = true;
                                else if (hitObject.X >= 182 && hitObject.X <= 255)
                                    hitObject.Key3 = true;
                                else if (hitObject.X >= 256 && hitObject.X <= 328)
                                    hitObject.Key4 = true;
                                else if (hitObject.X >= 329 && hitObject.X <= 401)
                                    hitObject.Key5 = true;
                                else if (hitObject.X >= 402 && hitObject.X <= 474)
                                    hitObject.Key6 = true;
                                else if (hitObject.X >= 475 && hitObject.X <= 547)
                                    hitObject.Key7 = true;
                            }
                            else
                            {
                                IsValid = false;
                            }

                            hitObject.Y = Int32.Parse(values[1], CultureInfo.InvariantCulture);
                            hitObject.StartTime = Int32.Parse(values[2], CultureInfo.InvariantCulture);
                            hitObject.Type = Int32.Parse(values[3], CultureInfo.InvariantCulture);
                            hitObject.HitSound = Int32.Parse(values[4], CultureInfo.InvariantCulture);
                            hitObject.Additions = "0:0:0:0:";

                            // If it's an LN, we'll want to add the object's EndTime as well.
                            if (line.Contains("128"))
                            {
                                var endTime = values[5].Substring(0, values[5].IndexOf(":"));
                                hitObject.EndTime = Int32.Parse(endTime, CultureInfo.InvariantCulture);
                            }

                            HitObjects.Add(hitObject);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                IsValid = false;
            }

        }
    }

    /// <summary>
    ///     Struct for the timing point data.
    /// </summary>
    public struct TimingPoint
    {
        public float Offset { get; set; }
        public float MillisecondsPerBeat { get; set; }
        public int Meter { get; set; }
        public int SampleType { get; set; }
        public int SampleSet { get; set; }
        public int Volume { get; set; }
        public int Inherited { get; set; }
        public int KiaiMode { get; set; }
    }

    /// <summary>
    ///  Struct for all the hit object data.
    /// </summary>
    public struct HitObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int StartTime { get; set; }
        public int Type { get; set; }
        public int HitSound { get; set; }
        public int EndTime { get; set; }
        public string Additions { get; set; }
        public bool Key1 { get; set; }
        public bool Key2 { get; set; }
        public bool Key3 { get; set; }
        public bool Key4 { get; set; }
        public bool Key5 { get; set; }
        public bool Key6 { get; set; }
        public bool Key7 { get; set; }
    }
}