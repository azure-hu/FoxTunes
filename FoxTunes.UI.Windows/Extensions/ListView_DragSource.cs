﻿using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FoxTunes
{
    public static partial class ListViewExtensions
    {
        private static readonly ConcurrentDictionary<ListView, DragSourceBehaviour> DragSourceBehaviours = new ConcurrentDictionary<ListView, DragSourceBehaviour>();

        public static readonly DependencyProperty DragSourceProperty = DependencyProperty.RegisterAttached(
            "DragSource",
            typeof(bool),
            typeof(ListViewExtensions),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnDragSourcePropertyChanged))
        );

        public static bool GetDragSource(ListView source)
        {
            return (bool)source.GetValue(DragSourceProperty);
        }

        public static void SetDragSource(ListView source, bool value)
        {
            source.SetValue(DragSourceProperty, value);
        }

        private static void OnDragSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ListView = sender as ListView;
            if (ListView == null)
            {
                return;
            }
            if (GetDragSource(ListView))
            {
                DragSourceBehaviours.TryAdd(ListView, new DragSourceBehaviour(ListView));
            }
            else
            {
                DragSourceBehaviours.TryRemove(ListView);

            }
        }

        public static readonly RoutedEvent DragSourceInitializedEvent = EventManager.RegisterRoutedEvent(
            "DragSourceInitialized",
            RoutingStrategy.Bubble,
            typeof(DragSourceInitializedEventHandler),
            typeof(ListViewExtensions)
        );

        public static void AddDragSourceInitializedHandler(DependencyObject source, DragSourceInitializedEventHandler handler)
        {
            var element = source as UIElement;
            if (element != null)
            {
                element.AddHandler(DragSourceInitializedEvent, handler);
            }
        }

        public static void RemoveDragSourceInitializedHandler(DependencyObject source, DragSourceInitializedEventHandler handler)
        {
            var element = source as UIElement;
            if (element != null)
            {
                element.RemoveHandler(DragSourceInitializedEvent, handler);
            }
        }

        public delegate void DragSourceInitializedEventHandler(object sender, DragSourceInitializedEventArgs e);

        public class DragSourceInitializedEventArgs : RoutedEventArgs
        {
            public DragSourceInitializedEventArgs(object data)
            {
                this.Data = data;
            }

            public DragSourceInitializedEventArgs(RoutedEvent routedEvent, object data)
                : base(routedEvent)
            {
                this.Data = data;
            }

            public DragSourceInitializedEventArgs(RoutedEvent routedEvent, object source, object data)
                : base(routedEvent, source)
            {
                this.Data = data;
            }

            public object Data { get; private set; }
        }

        private class DragSourceBehaviour
        {
            public DragSourceBehaviour(ListView listView)
            {
                this.ListView = listView;
                this.ListView.PreviewMouseDown += this.OnMouseDown;
                this.ListView.MouseMove += this.OnMouseMove;
            }

            public Point DragStartPosition { get; private set; }

            public bool DragInitialized { get; private set; }

            public ListView ListView { get; private set; }

            protected virtual bool ShouldInitializeDrag(object source, Point position)
            {
                if (this.DragStartPosition.Equals(default(Point)) || source is Thumb)
                {
                    return false;
                }
                if (Math.Abs(position.X - this.DragStartPosition.X) > SystemParameters.MinimumHorizontalDragDistance)
                {
                    return true;
                }
                if (Math.Abs(position.Y - this.DragStartPosition.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    return true;
                }
                return false;
            }

            protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    return;
                }
                this.DragStartPosition = e.GetPosition(null);
            }

            protected virtual void OnMouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton != MouseButtonState.Pressed || this.DragInitialized)
                {
                    return;
                }
                var selectedItems = GetSelectedItems(this.ListView);
                if (selectedItems == null || selectedItems.Count == 0)
                {
                    return;
                }
                var position = e.GetPosition(null);
                if (this.ShouldInitializeDrag(e.OriginalSource, position))
                {
                    this.DragStartPosition = default(Point);
                    this.ListView.RaiseEvent(new DragSourceInitializedEventArgs(DragSourceInitializedEvent, selectedItems));
                }
            }
        }
    }
}