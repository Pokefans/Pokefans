// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Linq;
using System.Web;
using DiffMatchPatch;
using Pokefans.Data;

namespace Pokefans.Util
{
    public class ContentManager
    {
        private readonly Entities _entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class.
        /// </summary>
        /// <param name="entities">The entities instace</param>
        public ContentManager(Entities entities)
        {
            _entities = entities;
        }

        /// <summary>
        /// Create a new content in the cms for the given author
        /// </summary>
        /// <param name="author">The user to create the content for</param>
        /// <returns>The created content object</returns>
        public Content CreateContent(User author)
        {
            var content = new Content
            {
                Version = 1,
                EditPermissionId = null,
                Published = DateTime.MinValue,
                PublishedByUserId = null,
                Created = DateTime.Now,
                AuthorUserId = author.Id,
                UnparsedContent = "",
                StylesheetCss = ""
            };

            content = _entities.Contents.Add(content);

            var contentVersion = new ContentVersion
            {
                ContentId = content.Id,
                UserId = content.AuthorUserId,
                UnparsedContent = "",
                StylesheetCode = "",
                Title = "",
                Description = "",
                Teaser = "",
                Notes = ""
            };

            _entities.ContentVersions.Add(contentVersion);

            var contentUrl = new ContentUrl
            {
                Url = $"inhalt/{content.Id}",
                ContentId = content.Id,
                Type = UrlType.System,
                Enabled = true
            };

            _entities.ContentUrls.Add(contentUrl);
            content.DefaultUrl = contentUrl;

            _entities.SaveChanges();
            return content;
        }

        /// <summary>
        /// Update version information for a content object
        /// </summary>
        /// <param name="content">The content to update the versions for</param>
        /// <param name="author">The user who authored the changes</param>
        /// <returns></returns>
        public ContentVersion UpdateContentVersions(Content content, User author)
        {
            var latestVersion = content.GetLatestVersion();
            var latesetUserId = latestVersion.UserId;
            UpdateMagnificance changes;

            if (latesetUserId != author.Id)
            {
                changes = CalculateUpdateMagnificance(latestVersion.UnparsedContent, content.UnparsedContent);

                // If the user was not the one who updated last we have to create a new version
                content.Version++;
                latestVersion = new ContentVersion
                {
                    ContentId = content.Id,
                    Version = content.Version,
                    UserId = author.Id,
                    Note = content.Notes,
                };

                _entities.ContentVersions.Add(latestVersion);
            }
            else
            {
                // Calculate changes made in comparison the the previous version. If this is the first version
                // we compare to an empty string.
                var previousVersion = content.GetLatestVersion(1);
                var previousContent = previousVersion == null ? string.Empty : previousVersion.UnparsedContent;
                changes = CalculateUpdateMagnificance(previousContent, content.UnparsedContent);
            }

            latestVersion.Title = content.Title;
            latestVersion.UnparsedContent = content.UnparsedContent;
            latestVersion.StylesheetCode = content.StylesheetCode;
            latestVersion.Description = content.Description;
            latestVersion.Teaser = content.Teaser;
            latestVersion.Notes = content.Notes;

            latestVersion.Version = content.Version;
            latestVersion.ParsedContent = content.ParsedContent;
            latestVersion.StylesheetCss = content.StylesheetCss;
            latestVersion.Updated = DateTime.Now;
            latestVersion.UpdateMagnificance = changes.updateMagnificance;
            latestVersion.UpdateCharsChanged = changes.updateCharsInserted;
            latestVersion.UpdateCharsDeleted = changes.updateCharsDeleted;

            // Save all Changes
            _entities.SaveChanges();

            return latestVersion;
        }

        /// <summary>
        /// Structure for passing an update's magnificance
        /// </summary>
        struct UpdateMagnificance
        {
            public double updateMagnificance;
            public int updateCharsDeleted;
            public int updateCharsInserted;
        }

        /// <summary>
        /// Calculate the magnificance of a content update on a scale from 0 to 1.
        /// </summary>
        /// <param name="sourceContent">The content before the changes were made</param>
        /// <param name="targetContent">The content after the changes were made</param>
        /// <returns>The changes' magnificance</returns>
        private static UpdateMagnificance CalculateUpdateMagnificance(string sourceContent, string targetContent)
        {
            // Calculate Changes
            int updateCharsDeleted = 0;
            int updateCharsInserted = 0;
            double updateMagnificance = 0;

            var dmp = new diff_match_patch();
            var diffs = dmp.diff_main(sourceContent, targetContent);

            foreach (var diffItem in diffs.Where(diffItem => diffItem.text.Trim().Length != 0))
            {
                switch (diffItem.operation)
                {
                    case Operation.DELETE:
                        {
                            updateCharsDeleted += diffItem.text.Length;
                            break;
                        }
                    case Operation.INSERT:
                        {
                            updateCharsInserted += diffItem.text.Length;
                            break;
                        }
                }
            }

            updateMagnificance =
                Math.Min(updateCharsInserted * 0.9 / Math.Max(1, sourceContent.Length) +
                            updateCharsDeleted * 0.1 / Math.Max(1, sourceContent.Length), 1);
            updateMagnificance = Math.Min(1, Math.Tan(updateMagnificance));

            return new UpdateMagnificance
            {
                updateMagnificance = updateMagnificance,
                updateCharsDeleted = updateCharsDeleted,
                updateCharsInserted = updateCharsInserted
            };
        }
    }
}
