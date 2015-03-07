using EntityModel;
using EntityModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocAsCode.BuildMeta
{
    public static class YamlMetadataResolver
    {
        public static YamlViewModel ResolveMetadata(Dictionary<string, YamlItemViewModel> allMembers)
        {
            // Folder structure
            // toc.yaml # toc containing all the namespaces
            // api.yaml # id-yaml mapping
            // api/{id}.yaml # items
            const string YamlExtension = ".yaml";
            const string TocYamlFileName = "toc" + YamlExtension;
            const string ApiFolder = "api";
            YamlViewModel viewModel = new YamlViewModel();
            viewModel.IndexYamlViewModel = new Dictionary<string, IndexYamlItemViewModel>();
            viewModel.TocYamlViewModel = new YamlItemViewModel()
            {
                Type = MemberType.Toc,
                Items = new List<YamlItemViewModel>(),
                YamlPath = TocYamlFileName
            };
            viewModel.MemberYamlViewModelList = new List<YamlItemViewModel>();
            foreach (var member in allMembers.Values)
            {
                if (member.Type.IsPageLevel())
                {
                    // Add to member yaml list only if the member is in page level
                    viewModel.MemberYamlViewModelList.Add(member);

                    // If it is a page, follow the convention to generate the YamlPath
                    member.YamlPath = Path.Combine(ApiFolder, member.Name + YamlExtension);
                    member.Href = Path.Combine(ApiFolder, member.Name);
                    if (member.Type == MemberType.Namespace)
                    {
                        // Add to index
                        viewModel.IndexYamlViewModel.Add(member.Name, new IndexYamlItemViewModel { Name = member.Name, YamlPath = member.YamlPath, Href = member.Href });
                        // Use the shrinked version of child
                        viewModel.TocYamlViewModel.Items.Add(member.ShrinkSelfAndChildren());
                    }
                    else
                    {
                        viewModel.IndexYamlViewModel.Add(member.Name, new IndexYamlItemViewModel { Name = member.Name, YamlPath = member.YamlPath, Href = member.Href });
                        if (member.Items != null)
                        {
                            // For those inside a page, e.g. method inside a class, inherit the owned classes href and YamlPath
                            foreach (var i in member.Items)
                            {
                                Debug.Assert(!i.Type.IsPageLevel());
                                if (!i.Type.IsPageLevel())
                                {
                                    i.YamlPath = member.YamlPath;
                                    i.Href = member.Href;

                                    // Add to index
                                    viewModel.IndexYamlViewModel.Add(i.Name, new IndexYamlItemViewModel { Name = i.Name, YamlPath = i.YamlPath, Href = i.Href });
                                }
                            }
                        }
                    }
                }
            }
            return viewModel;
        }

        
    }

    public interface IResolverPipeline
    {
        void ResolveMetadata(YamlItemViewModel item, Dictionary<string, YamlItemViewModel> allMembers);
    }

    public class ResolveLink
    {

    }

    public class ResolveRelativePath
    {

    }

    public class ResolveGitPath
    {

    }
}
