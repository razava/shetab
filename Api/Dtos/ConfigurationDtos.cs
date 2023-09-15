using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    //public class ProcessGetDto
    //{
    //    public string Title { get; set; }
    //    public int Id { get; set; }
    //    public ICollection<StageDto> Stages { get; set; } = new List<StageDto>();
    //    public ICollection<TransitionDto> Transitions { get; set; } = new List<TransitionDto>();
    //}

    public class ProcessCreateDto
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public ICollection<StageDto> Stages { get; set; } = new List<StageDto>();
        public ICollection<TransitionDto> Transitions { get; set; } = new List<TransitionDto>();
    }
}
