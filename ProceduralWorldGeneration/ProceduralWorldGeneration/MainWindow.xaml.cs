﻿using ProceduralWorldGeneration.Elements;
using ProceduralWorldGeneration.Generator;
using ProceduralWorldGeneration.Input;
using ProceduralWorldGeneration.Input.LexerDefinition;
using ProceduralWorldGeneration.Input.ParserDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProceduralWorldGeneration
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorldGenerator world_generator;
        WorldGenerationConfig config;
        MythCreator myth_creator;
        MythObjectReader myth_object_reader;
        Parser myth_object_parser;

        public MainWindow()
        {
            InitializeComponent();
            world_generator = new WorldGenerator();
            config = new WorldGenerationConfig();
            myth_creator = new MythCreator();
            SeedTextBox.DataContext = config;

            myth_object_reader = new MythObjectReader();
            myth_object_reader.readMythObjects();

            foreach(Token t in myth_object_reader.Tokens)
            {
                UpdateGenerationLog("Token: " + t.Value + " Type: " +  t.Type + "\n");
            }
            myth_object_parser = new Parser();
            myth_object_parser.generateExpressionTree(myth_object_reader.Tokens);

            world_generator.createdNewElement += new WorldGenerator.CreatedNewElement(UpdateGenerationLog);
            world_generator.endedGeneration += new WorldGenerator.EndedGeneration(UpdateGenerationLog);
            world_generator.endedGeneration += new WorldGenerator.EndedGeneration(WorldGenerationButton_Enable);

            world_generator.InitializeWorldGenerator(config);
            ElementListView.DataContext = world_generator;
            ElementListView.ItemsSource = world_generator.GeneratedWorld.ElementCollection;
        }

        private void WorldGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            WorldGenerationButton.IsEnabled = false;
            world_generator.generateWorld();
        }

        private void WorldGenerationButton_Enable(string status)
        {
            this.WorldGenerationButton.IsEnabled = true;
        }

        private void UpdateGenerationLog(string status)
        {
            GenerationLog.AppendText(status);
        }

        private void ElementListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Element element = (Element)e.AddedItems[0];

            NameDisplayTextBlock.Text = element.Name;
            SizeDisplayTextBlock.Text = element.Size.ToString();
            if (element.ParentElement != null)
                ParentNameDisplayTextBlock.Text = element.ParentElement.Name;
            else
                ParentNameDisplayTextBlock.Text = "NONE";

            ChildrenElementListBox.DataContext = element;
            ChildrenElementListBox.ItemsSource = element.ChildElements;
        }
    }
}
