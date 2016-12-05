select '<url><loc>http://probanki.net/</loc></url>' as urls
union
select '<url><loc>http://probanki.net/publikacii</loc></url>' as urls
union
select '<url><loc>http://probanki.net/publikacii/' + cat.Alias + '</loc></url>' as urls
from Categories cat
union
select '<url><loc>http://probanki.net/publikacii/' + cat.Alias + '/' + a.Alias + '</loc></url>' as urls
from Articles a join Categories cat on a.CategoryId = cat.Id
union
select '<url><loc>http://probanki.net/orgs</loc></url>' as urls
union
select '<url><loc>http://probanki.net/orgs/' + cat.Alias + '</loc></url>' as urls
from OrgCategories cat
union
select '<url><loc>http://probanki.net/orgs/' + cat.Alias + '/' + o.Alias + '</loc></url>' as urls
from Orgs o join OrgCategories cat on o.CategoryId = cat.Id
union
select '<url><loc>http://probanki.net/news</loc></url>' as urls
union
select '<url><loc>http://probanki.net/news/' + n.Alias + '</loc></url>' as urls
from News n
order by urls