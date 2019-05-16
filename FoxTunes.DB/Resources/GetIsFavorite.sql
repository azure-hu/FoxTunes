SELECT CASE WHEN EXISTS(
	SELECT *
	FROM "LibraryItems"
		JOIN "LibraryHierarchyItem_LibraryItem"
			ON "LibraryItems"."Id" = "LibraryHierarchyItem_LibraryItem"."LibraryItem_Id"
	WHERE "LibraryHierarchyItem_LibraryItem"."LibraryHierarchyItem_Id" = @libraryHierarchyItemId
		AND "LibraryItems"."Favorite" = 1
) THEN 1 ELSE 0 END