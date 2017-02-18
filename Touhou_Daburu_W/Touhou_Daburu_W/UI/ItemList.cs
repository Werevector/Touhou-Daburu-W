using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Touhou_Daburu_W.UI
{
    public enum ItemListType
    {
        Horizontal,
        Vertical
    }

    class ItemList : MenuItem
    {
        protected IMenuItem mParent;
        protected List<IMenuItem> mItems;
        protected ItemListType mType;
        public Vector2 itemPad { get; set; }

        public ItemList(IMenuItem parent, Vector2 pos, ItemListType type, List<IMenuItem> list = null)
        {
            mParent = parent;
            mPosition = pos;
            mType = type;
            itemPad = new Vector2(10,10);
            mItems = new List<IMenuItem>();
            if (list != null)
                SetItemList(list);
        }

        public void SetItemList(List<IMenuItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
            CalculateListRectDiag();
        }

        public override void Render(SpriteBatch sb, SpriteFont font, Color color)
        {
            for (int i = 0; i < mItems.Count; i++)
                mItems[i].Render(sb, font, color);
        }

        public void AddItem(IMenuItem item)
        {
            item.SetParent(this);

            if (mItems.Count != 0)
            {
                IMenuItem previous = mItems[mItems.Count - 1];
                switch (mType)
                {
                    case ItemListType.Horizontal:
                        
                        item.SetPosition(previous.GetPosition().X + previous.GetItemSpan().X + itemPad.X, mPosition.Y + itemPad.Y);
                        break;
                    case ItemListType.Vertical:
                        item.SetPosition(mPosition.X + itemPad.X, previous.GetPosition().Y + previous.GetItemSpan().Y + itemPad.Y);
                        break;
                    default:
                        break;
                } 
            }
            else
            {
                item.SetPosition(mPosition + itemPad);
            }
            mItems.Add(item);
            CheckDiags(ref mItemRectDiag, item.GetItemSpan());
        }

        protected void CalculateListRectDiag()
        {
            Vector2 longest = new Vector2();
            foreach (var item in mItems)
            {
                Vector2 span = item.GetItemSpan();
                CheckDiags(ref longest, span);
            }
            mItemRectDiag = longest;
        }

        protected void CheckDiags(ref Vector2 diag, Vector2 span)
        {
            switch (mType)
            {
                case ItemListType.Horizontal:
                    if (diag.Y < span.Y)
                        diag.Y = span.Y;
                    diag.X += span.X;
                    break;
                case ItemListType.Vertical:
                    if (diag.X < span.X)
                        diag.X = span.X;
                    diag.Y += span.Y;
                    break;
                default:
                    break;
            }
        }

    }
}
