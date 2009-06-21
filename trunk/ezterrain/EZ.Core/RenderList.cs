using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public class RenderList : ICollection<IRenderable>
	{
		SortedList<RenderGroup, ICollection<IRenderable>> renderLists;

		public RenderList()
		{
			renderLists = new SortedList<RenderGroup, ICollection<IRenderable>>();
			renderLists.Add(RenderGroup.HUD, new List<IRenderable>());
			renderLists.Add(RenderGroup.Transparent, new List<IRenderable>());
			renderLists.Add(RenderGroup.Opaque, new List<IRenderable>());
		}

		public void Render(RenderInfo renderInfo)
		{
			IEnumerable<IRenderable> noninitialized = from renderable in this
													  where !renderable.Initialized
													  select renderable;

			foreach (IRenderable renderable in noninitialized)
			{
				renderable.Initialize();
			}

			IEnumerable<IRenderable> renderList = from renderable in this
												  where renderable.Update(renderInfo)
												  select renderable;

			foreach (IRenderable renderable in renderList)
			{
				renderable.Render(renderInfo);
			}
		}

		#region ICollection<IRenderable> Members

		public void Add(IRenderable item)
		{
			renderLists[item.RenderGroup].Add(item);
		}

		public void Clear()
		{
			renderLists.Clear();
		}

		public bool Contains(IRenderable item)
		{
			return renderLists[item.RenderGroup].Contains(item);
		}

		public void CopyTo(IRenderable[] array, int arrayIndex)
		{
			foreach (IRenderable renderable in this)
			{
				array[arrayIndex++] = renderable;
			}
		}

		public int Count
		{
			get { return renderLists.Aggregate(0, (total, pair) => total + pair.Value.Count); }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(IRenderable item)
		{
			return renderLists[item.RenderGroup].Remove(item);
		}

		#endregion

		#region IEnumerable<IRenderable> Members

		public IEnumerator<IRenderable> GetEnumerator()
		{
			foreach (KeyValuePair<RenderGroup, ICollection<IRenderable>> renderListPair in renderLists)
			{
				foreach (IRenderable renderable in renderListPair.Value)
				{
					yield return renderable;
				}
			}
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
