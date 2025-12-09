using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EntityBuilder.Metadata;

namespace EntityBuilder
{
    [CustomPropertyDrawer(typeof(ComponentBuilder))]
    public class BuilderDrawer : PropertyDrawer
    {
        private static EntityView _targetObject;
        private static HashSet<Type> _existingTypes;
        private static List<Type> _authoringTypes;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _existingTypes ??= new HashSet<Type>();
            _authoringTypes ??= new List<Type>();

            var value = property.managedReferenceValue;
            var viewObject = property.serializedObject.targetObject as EntityView;

            if (viewObject == null)
            {
                base.OnGUI(position, property, label);
                return;
            }

            if (viewObject != _targetObject)
            {
                _existingTypes.Clear();
                _targetObject = viewObject;

                if (viewObject.Builders != null)
                {
                    foreach (var builder in viewObject.Builders)
                    {
                        if (builder != null)
                        {
                            _existingTypes.Add(builder.GetType());
                        }
                    }
                }
            }

            if (value == null)
            {
                // button to select authoring
                if (GUI.Button(position, "Select Builder"))
                {
                    _authoringTypes.Clear();
                    var assembly = typeof(ComponentBuilder).Assembly;
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (CheckType(type))
                        {
                            _authoringTypes.Add(type);
                        }
                    }

                    var menu = new GenericMenu();
                    foreach (var type in _authoringTypes)
                    {
                        menu.AddItem(new GUIContent(type.Name), false, () =>
                        {
                            property.managedReferenceValue = Activator.CreateInstance(type);
                            property.serializedObject.ApplyModifiedProperties();
                        });
                    }

                    menu.ShowAsContext();
                }
            }
            else
            {
                // display selected authoring
                // var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                // EditorGUI.LabelField(rect, value.GetType().Name);

                var propertyType = value.GetType();
                EditorGUI.PropertyField(position, property, new GUIContent(propertyType.Name), true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var mrId = property.managedReferenceId;
            var mrFt = property.managedReferenceFieldTypename;

            return property.managedReferenceValue == null ? EditorGUIUtility.singleLineHeight : EditorGUI.GetPropertyHeight(property, label);
        }

        private bool CheckType(Type type)
        {
            return type != typeof(DefaultNonGenericBuilder)
                && type != typeof(DefaultBuilder<>)
                && !_existingTypes.Contains(type)
                && type.IsSubclassOf(typeof(ComponentBuilder));
        }
    }
}
