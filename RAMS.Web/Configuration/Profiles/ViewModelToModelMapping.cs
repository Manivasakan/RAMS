﻿using AutoMapper;
using RAMS.Enums;
using RAMS.Models;
using RAMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Web.Configuration
{
    /// <summary>
    /// ViewModelToModelMapping profile allows to configure view model to domain model mapping
    /// </summary>
    public class ViewModelToModelMapping : Profile
    {
        public ViewModelToModelMapping() : base("ViewModelToModelMapping") { }

        /// <summary>
        /// Configure method contains detailed configuration of each mapping 
        /// </summary>
        protected override void Configure()
        {
            /***** USER MAPPING *****/

            Mapper.CreateMap<AgentAddViewModel, Agent>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole))).ForMember(a => a.AgentStatus, map => map.MapFrom(vm => (AgentStatus)Enum.Parse(typeof(AgentStatus), vm.SelectedAgentStatus)));
            Mapper.CreateMap<ClientAddViewModel, Client>();
            Mapper.CreateMap<AdminAddViewModel, Admin>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole)));

            Mapper.CreateMap<AgentEditViewModel, Agent>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole))).ForMember(a => a.AgentStatus, map => map.MapFrom(vm => (AgentStatus)Enum.Parse(typeof(AgentStatus), vm.SelectedAgentStatus)));
            Mapper.CreateMap<ClientEditViewModel, Client>().ForMember(c => c.Role, map => map.MapFrom(vm => Role.Employee));
            Mapper.CreateMap<AdminEditViewModel, Admin>().ForMember(a => a.Role, map => map.MapFrom(vm => (Role)Enum.Parse(typeof(Role), vm.SelectedRole)));

            Mapper.CreateMap<UserEditProfileViewModel, Agent>().ForMember(a => a.AgentId, map => map.MapFrom(vm => vm.UserId));
            Mapper.CreateMap<UserEditProfileViewModel, Client>().ForMember(c => c.ClientId, map => map.MapFrom(vm => vm.UserId));
            Mapper.CreateMap<UserEditProfileViewModel, Admin>().ForMember(a => a.AdminId, map => map.MapFrom(vm => vm.UserId));

            /***** END OF USER MAPPING *****/
            /***** DEPARTMENT MAPPING *****/

            Mapper.CreateMap<DepartmentAddViewModel, Department>();
            Mapper.CreateMap<DepartmentEditViewModel, Department>();

            /***** END OF DEPARTMENT MAPPING *****/
            /***** POSITION MAPPING *****/

            Mapper.CreateMap<PositionAddViewModel, Position>();
            Mapper.CreateMap<PositionEditViewModel, Position>();

            /***** END OF POSITION MAPPING *****/
            /***** NOTIFICATION MAPPING *****/

            Mapper.CreateMap<NotificationAddViewModel, Notification>();

            /***** END OF NOTIFICATION MAPPING *****/
            /***** CATEGORY MAPPING *****/

            Mapper.CreateMap<CategoryAddViewModel, Category>();
            Mapper.CreateMap<CategoryEditViewModel, Category>();

            /***** END OF CATEGORY MAPPING *****/
        }
    }
}