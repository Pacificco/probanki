select 'http://probanki.net/' + cat.Alias +  a.Alias
from Articles a join Categories cat on a.CategoryId = cat.Id
order by cat.Id, a.CreatedAt desc;