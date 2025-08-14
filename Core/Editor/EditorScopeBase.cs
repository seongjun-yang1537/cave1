using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public abstract class EditorScopeBase<TScope, TModel, TModelData, TModelState> : Editor
        where TScope : Object
        where TModel : class
        where TModelData : class
        where TModelState : class
    {
        InspectorFactory<TModel> modelInspectorFactory;
        InspectorFactory<TModelData> modelDataInspectorFactory;
        InspectorFactory<TModelState> modelStateInspectorFactory;

        TScope scope;

        bool foldModel;
        bool foldModelData;
        bool foldModelState;

        protected virtual void OnEnable()
        {
            scope = (TScope)target;
            modelInspectorFactory = new InspectorFactory<TModel>();
            modelDataInspectorFactory = new InspectorFactory<TModelData>();
            modelStateInspectorFactory = new InspectorFactory<TModelState>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SEditorGUI.ChangeCheck(
                scope,
                SEditorGUILayout.Vertical("box")
                .Content(
                    RenderModel(GetModel(scope))
                    + RenderModelData(GetModelData(scope))
                    + RenderModelState(GetModelState(scope))
                )
            ).Render();
        }

        SUIElement RenderModel(TModel model)
        {
            if (model == null || !Application.isPlaying) return SUIElement.Empty();
            return SEditorGUILayout.FoldGroup("Model", foldModel)
            .OnValueChanged(value => foldModel = value)
            .Content(
                modelInspectorFactory.Render(model)
            );
        }

        SUIElement RenderModelState(TModelState modelState)
        {
            if (modelState == null || Application.isPlaying) return SUIElement.Empty();
            return SEditorGUILayout.FoldGroup("Model State", foldModelState)
            .OnValueChanged(value => foldModelState = value)
            .Content(
                modelStateInspectorFactory.Render(modelState)
            );
        }

        SUIElement RenderModelData(TModelData modelData)
        {
            if (modelData == null || Application.isPlaying) return SUIElement.Empty();
            return SEditorGUILayout.FoldGroup("Model Data", foldModelData)
            .OnValueChanged(value => foldModelData = value)
            .Content(
                modelDataInspectorFactory.Render(modelData)
            );
        }

        protected abstract TModel GetModel(TScope scope);
        protected abstract TModelData GetModelData(TScope scope);
        protected abstract TModelState GetModelState(TScope scope);
    }
}

