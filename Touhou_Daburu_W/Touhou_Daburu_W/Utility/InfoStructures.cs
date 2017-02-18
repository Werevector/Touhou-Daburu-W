using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touhou_Daburu_W
{
    struct AtlasInfo
    {
        public string Image { get; set; }
        public List<ClipSetInfo> ClipSets { get; set; }
    }
    struct EnemyAtlasInfo
    {
        public string Image { get; set; }
        public List<ClipSetInfo> ClipSets { get; set; }
        public List<SequenceList> EnemySequences { get; set; }
    }
    struct PlayerAtlasInfo
    {
        public string Image { get; set; }
        public List<ClipSetInfo> ClipSets { get; set; }
        public List<SequenceInfo> Sequences { get; set; }
    }
    struct ClipSetInfo
    {
        public string Key { get; set; }
        public int OriginAngle { get; set; }
        public List<List<int>> Set { get; set; }
    }
    struct SequenceList
    {
        public string Key { get; set; }
        public List<SequenceInfo> Sequences;
    }
    struct SequenceInfo
    {
        public string Key { get; set; }
        public bool Looping { get; set; }
        public int SubLoop { get; set; }
        public List<int> Seq { get; set; }
    }
    struct EnemyInfo
    {
        public string SpriteType { get; set; }
        public List<int> HitBox { get; set; }
        public int Health { get; set; }
        public List<int> Position { get; set; }
        public float MoveSpeed { get; set; }
        public List<PatternInfo> FirePatterns { get; set; }
    }
    struct PatternInfo
    {
        public string SpriteType { get; set; }
        public int ColorIndex { get; set; }
        public int ArrayNumber { get; set; }
        public int BulletsPerArray { get; set; }
        public float InternalArraySpread { get; set; }
        public float ArraySpread { get; set; }
        public double PatternAngle { get; set; }
        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public double RotationSpeed { get; set; }
        public double RotationSpeedDelta { get; set; }
        public bool BoundedRotation { get; set; }
        public double MaxRotation { get; set; }
        public double MinRotation { get; set; }
        public bool FlipRotation { get; set; }
        public int FireRate { get; set; }
        public List<int> HitBox { get; set; }
        public bool Directional { get; set; }
    }
    struct GeneratorInfo
    {
        public int Amount { get; set; }
        public float Time { get; set; }
        public float Interval { get; set; }
        public int[][] PathControlPoints { get; set; }
        public EnemyInfo Enemy { get; set; }
    }
    struct StageInfo
    {
        public List<GeneratorInfo> Generators { get; set; }
    }
}
