using System;
using Corelib.Utils;
using TriInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class EntityScope : LifetimeScope
    {
        #region ========== Input ==========
        [SerializeReference, SubclassSelector]
        public EntityModelData entityModelData;
        [SerializeReference, SubclassSelector]
        public EntityModelState entityModelState;
        public bool isUseState;

        [ShowInInspector]
        public Type ModelType => GetModelType();

        [SerializeReference, SubclassSelector]
        public IEntityInteractable entityInteractable;
        #endregion ====================

        #region ========== Runtime ==========
        [HideInInspector]
        public EntityModel entityModel;
        #endregion ====================

        #region ========== Prefab External ==========
        [NonSerialized]
        public Func<EntityModel> onCreateModel;
        #endregion ====================

        public bool isAutoInjectModel;
        public bool isAutoInjectController;

        protected override void Configure(IContainerBuilder builder)
        {
            entityModel = onCreateModel?.Invoke() ?? CreateModel();

            builder.RegisterComponent(transform);

            if (TryGetComponent<Rigidbody>(out var rigidbody))
                builder.RegisterComponent(rigidbody).AsSelf();

            builder.RegisterComponent(GetComponent<EntityView>()).AsSelf();

            RegisterInteractable(builder);

            if (isAutoInjectModel)
                RegisterModel(builder);

            if (isAutoInjectController)
                RegisterController(builder);
        }

        protected virtual void RegisterInteractable(IContainerBuilder builder)
        {
            entityInteractable ??= new NoneEntityInteractable();
            builder.RegisterInstance(entityInteractable).As<IEntityInteractable>();
        }

        protected virtual void RegisterModel(IContainerBuilder builder) =>
            builder.RegisterInstance(entityModel).As<EntityModel>().AsSelf();

        private EntityModel CreateModel()
        {
            if (entityModelData == null)
                return null;

            var targetState = isUseState ? entityModelState : null;
            return targetState != null
                ? EntityModelFactory.Create(entityModelData, targetState)
                : EntityModelFactory.Create(entityModelData);
        }

        protected virtual void RegisterController(IContainerBuilder builder) =>
            builder.RegisterComponent(GetComponent<EntityController>()).AsSelf();

        private Type GetModelType()
        {
            if (isUseState && entityModelState != null)
                return ExType.LCAType(entityModelData.TargetType, entityModelState.TargetType);

            return entityModelData?.TargetType ?? typeof(object);
        }
    }
}