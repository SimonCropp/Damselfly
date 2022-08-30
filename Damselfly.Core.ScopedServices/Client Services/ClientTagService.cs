﻿using System;
using Damselfly.Core.DbModels;
using System.Net.Http;
using Damselfly.Core.Models;
using System.Net.Http.Json;
using Damselfly.Core.Constants;
using Damselfly.Core.ScopedServices.Interfaces;
using Damselfly.Core.ScopedServices.ClientServices;

namespace Damselfly.Core.ScopedServices;

public class ClientTagService : ITagService, IRecentTagService, ITagSearchService
{
    private readonly RestClient httpClient;
    private readonly NotificationsService _notifications;

    private ICollection<Tag> _favouriteTags;
    private ICollection<string> _recentTags;
    public event Action OnFavouritesChanged;

    public ClientTagService(RestClient client, NotificationsService notifications )
    {
        httpClient = client;
        _notifications = notifications;

        _notifications.SubscribeToNotification(NotificationType.FavouritesChanged, OnFavouritesChanged);
    }

    public async Task<ICollection<Tag>> GetFavouriteTags()
    {
        // WASM: Cache here?
        _favouriteTags = await httpClient.CustomGetFromJsonAsync<List<Tag>>("/api/tags/favourites");

        return _favouriteTags;
    }

    public async Task<ICollection<string>> GetRecentTags()
    {
        // WASM: Cache here?
        _recentTags = await httpClient.CustomGetFromJsonAsync<List<string>>("/api/tags/recents");

        return _recentTags;
    }

    public async Task<bool> ToggleFavourite(Tag tag)
    {
        return await httpClient.CustomPostAsJsonAsync<Tag, bool>($"/api/tags/togglefave", tag);
    }

    public async Task UpdateTagsAsync(ICollection<Image> images, ICollection<string> tagsToAdd, ICollection<string> tagsToDelete, int? userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Tag>> SearchTags(string filterText)
    {
        return await httpClient.CustomGetFromJsonAsync<List<Tag>>($"/api/tags/search/{filterText}");
    }

    public async Task<ICollection<Tag>> GetAllTags()
    {
        return await httpClient.CustomGetFromJsonAsync<List<Tag>>($"/api/tags");
    }


    public Task SetExifFieldAsync(Image[] images, ExifOperation.ExifType exifType, string newValue, int? userId = -1)
    {
        throw new NotImplementedException();
    }
}

