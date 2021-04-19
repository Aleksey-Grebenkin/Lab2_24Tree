﻿using System.Collections.Generic;
using System.Linq;

namespace Lab2_24Tree
{
    class TwoThreeFourTree
    {
        public TwoThreeFourTree Parent { get; private set; }

        private List<int> _values = new List<int>();

        public List<int> Values
        {
            get => _values;
            private set => _values = value.OrderBy(number => number).ToList();
        }

        private int Type => _values.Count + 1;

        public TwoThreeFourTree(int value, TwoThreeFourTree parent = null)
        {
            var values = Values;
            values.Add(value);
            Values = values;
            Parent = parent;
            Children = new TwoThreeFourTree[4];
        }

        public readonly TwoThreeFourTree[] Children;

        private bool IsParent => Parent == null;

        private bool IsLeaf
        {
            get { return Children.All(t => t == null); }
        }

        public static TwoThreeFourTree GetHead(TwoThreeFourTree tree)
        {
            if (tree == null) return null;

            var head = tree;

            while (!head.IsParent)
            {
                head = head.Parent;
            }

            return head;
        }

        private void SortChildren(TwoThreeFourTree tree)
        {
            for (int i=0;i<tree.Children.Length;i++)
                for (int j=0;j<tree.Children.Length;j++)
                    if (tree.Children[i]!=null && tree.Children[j]!=null)
                    {
                        if (tree.Children[i].Values.Count>0 && tree.Children[j].Values.Count>0)
                        {
                            if(i>j && tree.Children[i].Values[0]<tree.Children[j].Values[0])
                            {
                                var tmp = tree.Children[i];
                                tree.Children[i] = tree.Children[j];
                                tree.Children[j] = tmp;
                            }
                            if(i<j && tree.Children[i].Values[0]>tree.Children[j].Values[0])
                            {
                                var tmp = tree.Children[i];
                                tree.Children[i] = tree.Children[j];
                                tree.Children[j] = tmp;
                            }
                        }
                    }
        }
        public static void Add(int value, TwoThreeFourTree tree)
        {
            while (true)
            {
                switch (tree.Type)
                {
                    // наткнулись на 4-узел => разбваем его
                    case 4:
                        if (tree.IsParent)
                        {
                            //случай когда является корнем
                            var middle = tree.Values[1];
                            var vals = tree.Values;

                            //оставляем только центральное значение
                            var list = new List<int> {middle};
                            tree.Values = list;

                            //остальные значения раскидываем по новым детям 

                            var tree1 = new TwoThreeFourTree(vals[0], tree);
                            var tree2 = new TwoThreeFourTree(vals[2], tree);

                            // назначаем новых детей родителями для старых детей

                            //указываем их родителями

                            if (tree.Children[0] != null) tree.Children[0].Parent = tree1;
                            if (tree.Children[1] != null) tree.Children[1].Parent = tree1;
                            if (tree.Children[2] != null) tree.Children[2].Parent = tree2;
                            if (tree.Children[3] != null) tree.Children[3].Parent = tree2;

                            //указываем их детей
                            tree1.Children[0] = tree.Children[0];
                            tree1.Children[1] = tree.Children[1];
                            tree2.Children[0] = tree.Children[2];
                            tree2.Children[1] = tree.Children[3];

                            //Убираем всех предыдущих детей
                            for (var i = 0; i < tree.Children.Length; i++)
                            {
                                tree.Children[i] = null;
                            }

                            //Назначаем новых детей 

                            var k = 0;
                            for (var i = 0; i < tree.Children.Length; i++)
                            {
                                if (tree.Children[i] != null) continue;
                                tree.Children[i] = tree1;
                                k = i;
                                break;
                            }

                            for (var i = k; i < tree.Children.Length; i++)
                            {
                                if (tree.Children[i] != null) continue;
                                tree.Children[i] = tree2;
                                break;
                            }

                            /*
                            if (tree.Children[1] != null && tree.Children[2] != null &&
                                (tree.Children[0].Values[1] > tree.Children[2].Values[0]))
                            {
                                var tmp = tree.Children[2];
                                tree.Children[2] = tree.Children[1];
                                tree.Children[1] = tmp;
                            }
                            */
                            tree.SortChildren(tree);
                        }
                        else
                        {
                            //случай когда не является корнем

                            // добавляем центральное значение в список значений родителя
                            var valsParent = tree.Parent.Values;
                            valsParent.Add(tree.Values[1]);
                            tree.Parent.Values = valsParent;

                            var vals = tree.Values;

                            //удаляем текущий узел из детей родителя 

                            for (var i = 0; i < tree.Parent.Children.Length; i++)
                            {
                                if (tree != tree.Parent.Children[i]) continue;
                                tree.Parent.Children[i] = null;
                                break;
                            }

                            // крайние значения раскидываем по 2 новым узлам

                            var tree1 = new TwoThreeFourTree(vals[0], tree.Parent);
                            var tree2 = new TwoThreeFourTree(vals[2], tree.Parent);

                            //записываем новые узлы детьми к родителю
                            var k = 0;
                            for (var i = 0; i < tree.Children.Length; i++)
                            {
                                if (tree.Parent.Children[i] != null) continue;
                                tree.Parent.Children[i] = tree1;
                                k = i;
                                break;
                            }

                            for (var i = k; i < tree.Children.Length; i++)
                            {
                                if (tree.Parent.Children[i] != null) continue;
                                tree.Parent.Children[i] = tree2;
                                break;
                            }

                            tree.SortChildren(tree.Parent);
                            /*
                            if (tree.Parent.Children[1] != null && tree.Parent.Children[2] != null &&
                                (tree.Parent.Children[1].Values[0] > tree.Parent.Children[2].Values[0]))
                            {
                                var tmp = tree.Parent.Children[2];
                                tree.Parent.Children[2] = tree.Parent.Children[1];
                                tree.Parent.Children[1] = tmp;
                            }
                            */

                            //назначаем новые узлы родителями детям старого узла

                            //указываем их родителями
                            if (tree.Children[0] != null) tree.Children[0].Parent = tree1;
                            if (tree.Children[1] != null) tree.Children[1].Parent = tree1;
                            if (tree.Children[2] != null) tree.Children[2].Parent = tree2;
                            if (tree.Children[3] != null) tree.Children[3].Parent = tree2;

                            //указываем их детей
                            tree1.Children[0] = tree.Children[0];
                            tree1.Children[1] = tree.Children[1];

                            tree2.Children[0] = tree.Children[2];
                            tree2.Children[1] = tree.Children[3];

                            tree = tree.Parent;
                        }

                        break;
                    default:
                    {
                        if (tree.IsLeaf)
                        {
                            var values = tree.Values;
                            values.Add((int) value);
                            tree.Values = values;
                            return;
                        }
                    }
                        break;
                }

                //сюда попадаем после разбиения 4-узла
                var n = 0;
                for (var i = 0; i < tree.Values.Count; i++)
                {
                    if (value < tree.Values[i])
                    {
                        if (tree.Children[i] != null)
                            Add(value, tree.Children[i]);
                        else
                        {
                            tree.Children[i] = new TwoThreeFourTree(value, tree);
                        }

                        return;
                    }

                    n = i;
                }

                if (tree.Children[n + 1] != null)
                {
                    tree = tree.Children[n + 1];
                    continue;
                }

                tree.Children[n + 1] = new TwoThreeFourTree(value, tree);

                break;
            }
        }
    }
}