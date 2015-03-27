﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.UI.Common
{
    public class GroupCollection
    {
        private readonly List<Group> _groups = new List<Group>();

        public IReadOnlyList<Group> AllGroups { get { return new ReadOnlyCollection<Group>(_groups); } }

        public Group GetGroup(GroupType groupType, int ordinal)
        {
            var grp = _groups.SingleOrDefault(g => g.GroupType == groupType && g.Ordinal == ordinal);

            if (grp == null)
            {
                grp = new Group(groupType, ordinal);
                _groups.Add(grp);
            }

            return grp;
        }

        public IEnumerable<Group> GetGroupsByCell(Cell cell)
        {
            return _groups.Where(g => g.ContainedCells.Any(c => c == cell));
        }
    }
}
