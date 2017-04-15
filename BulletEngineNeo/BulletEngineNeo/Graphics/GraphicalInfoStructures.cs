using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletEngineNeo.Graphics
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
}
